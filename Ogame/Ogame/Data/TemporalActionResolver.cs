using System;
using Ogame.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;

namespace Ogame.Data
{
    public class TemporalActionResolver
    {
        private static readonly float defenseDamageMult = 100;
        private static readonly TimeSpan ProductionCycle = new TimeSpan(0, 0, 5, 0);


        private static int IActionHolderSort(IActionHolder a1, IActionHolder a2)
        {
            if (a1.Action == null)
                return -1;
            if (a2.Action == null)
                return 1;
            if (a1.Action.Due_to < a2.Action.Due_to)
                return -1;
            if (a1.Action.Due_to > a2.Action.Due_to)
                return 1;
            return 0;
        }

        public static void ResolveAction(ApplicationDbContext context, IActionHolder actionHolder, DateTime until)
        {
            switch (actionHolder.Action.Type)
            {
                case TemporalAction.ActionType.Attack:
                    HandleTemoralActionForUserUntil(context, actionHolder.Action.Target.UserID, actionHolder.Action.Due_to);
                    foreach (var defense in actionHolder.Action.Target.Defenses)
                    {
                        if (defense.Level > actionHolder.Level)
                        {
                            (actionHolder as Spaceship).Energy = 0;
                        }
                        (actionHolder as Spaceship).Energy -= defense.Level * defenseDamageMult;
                        if ((actionHolder as Spaceship).Energy < 0)
                        {
                            (actionHolder as Spaceship).Energy = 0;
                            break;
                        }
                    }
                    if ((actionHolder as Spaceship).Energy > 0)
                    {
                        if (actionHolder.Action.Target.UserID == null)
                        {
                            actionHolder.Action.Target.UserID = actionHolder.Planet.UserID;
                            context.Planets.Update(actionHolder.Action.Target);
                        }
                        else
                        {
                            float stealCristal = actionHolder.Action.Target.Cristal / 2;
                            actionHolder.Action.Target.Cristal -= stealCristal;
                            actionHolder.Planet.Cristal += stealCristal;
                            float stealDeuterium = actionHolder.Action.Target.Deuterium / 2;
                            actionHolder.Action.Target.Deuterium -= stealDeuterium;
                            actionHolder.Planet.Deuterium += stealDeuterium;
                            float stealMetal = actionHolder.Action.Target.Metal / 2;
                            actionHolder.Action.Target.Metal -= stealMetal;
                            actionHolder.Planet.Metal += stealMetal;
                            context.Planets.Update(actionHolder.Action.Target);
                            context.Planets.Update(actionHolder.Planet);
                        }
                    }
                    break;
                case TemporalAction.ActionType.Production:
                    actionHolder.Action.Due_to = actionHolder.Action.Due_to.Add(-ProductionCycle);
                    break;
                case TemporalAction.ActionType.Upgrade:
                    actionHolder.Level += 1;
                    context.Update(actionHolder);
                    break;
            }
            if (actionHolder is Mine || actionHolder is SolarPanel)
            {
                TimeSpan interval = until - actionHolder.Action.Due_to;
                int numCycle = (int)(interval / ProductionCycle);
                if (numCycle > 0)
                {
                    float produce = numCycle * actionHolder.Level * actionHolder.Caps.Growth_factor;
                    if (actionHolder is Mine)
                    {
                        produce *= ((Mine)actionHolder).Collect_rate;
                        switch (((Mine)actionHolder).Ressource)
                        {
                            case Mine.Ressources.Cristal:
                                if (((Mine)actionHolder).Planet.Cristal + produce > ((Mine)actionHolder).Caps.Cristal_cap * ((Mine)actionHolder).Level * ((Mine)actionHolder).Caps.Growth_factor)
                                {
                                    ((Mine)actionHolder).Planet.Cristal = ((Mine)actionHolder).Caps.Cristal_cap;
                                }
                                else
                                {
                                    ((Mine)actionHolder).Planet.Cristal += produce;
                                }
                                break;
                            case Mine.Ressources.Metal:
                                if (((Mine)actionHolder).Planet.Metal + produce > ((Mine)actionHolder).Caps.Metal_cap * ((Mine)actionHolder).Level * ((Mine)actionHolder).Caps.Growth_factor)
                                {
                                    ((Mine)actionHolder).Planet.Metal = ((Mine)actionHolder).Caps.Metal_cap;
                                }
                                else
                                {
                                    ((Mine)actionHolder).Planet.Metal += produce;
                                }
                                break;
                            case Mine.Ressources.Deuterium:
                                if (((Mine)actionHolder).Planet.Deuterium + produce > ((Mine)actionHolder).Caps.Deuterium_cap * ((Mine)actionHolder).Level * ((Mine)actionHolder).Caps.Growth_factor)
                                {
                                    ((Mine)actionHolder).Planet.Deuterium = ((Mine)actionHolder).Caps.Deuterium_cap;
                                }
                                else
                                {
                                    ((Mine)actionHolder).Planet.Deuterium += produce;
                                }
                                break;
                        }
                    }
                    else if (actionHolder is SolarPanel)
                    {
                        produce *= ((SolarPanel)actionHolder).Collect_rate;
                        if (((SolarPanel)actionHolder).Planet.Energy + produce > ((SolarPanel)actionHolder).Caps.Energy_cap)
                        {
                            ((SolarPanel)actionHolder).Planet.Energy = ((SolarPanel)actionHolder).Caps.Energy_cap * ((SolarPanel)actionHolder).Level * ((SolarPanel)actionHolder).Caps.Growth_factor;
                        }
                        else
                        {
                            ((SolarPanel)actionHolder).Planet.Energy += produce;
                        }
                    }
                    actionHolder.Action.Type = TemporalAction.ActionType.Production;
                    actionHolder.Action.Due_to = actionHolder.Action.Due_to.Add((numCycle + 1) * ProductionCycle);
                    context.Actions.Update(actionHolder.Action);
                    context.Planets.Update(actionHolder.Planet);
                }
            }
            else
            {
                actionHolder.Action.Type = TemporalAction.ActionType.Idle;
                actionHolder.Action.Due_to = until.Add(ProductionCycle);
                actionHolder.Action.TargetID = null;
                actionHolder.Action.Target = null;
            }
            context.SaveChanges();
        }

        public static void HandleTemoralActionForUserUntil(ApplicationDbContext context, string userId, DateTime? now = null)
        {
            if (now == null)
                now = DateTime.Now;

            Mine[] Minelist = context.Mines
                .Include(m => m.Planet)
                .Where(m => m.Planet.UserID == userId)
                .Include(m => m.Action)
                .Where(m => m.Action != null && m.Action.Due_to < now)
                .Include(m => m.Caps)
                .OrderBy(m => m.Action.Due_to)
                .ToArray();

            Defense[] DefenseList = context.Defenses
                .Include(m => m.Planet)
                .Where(m => m.Planet.UserID == userId)
                .Include(m => m.Action)
                .Where(m => m.Action != null && m.Action.Due_to < now)
                                .Include(m => m.Caps)

                .OrderBy(m => m.Action.Due_to)
                .ToArray();

            SolarPanel[] SolarList = context.SolarPanels
                .Include(m => m.Planet)
                .Where(m => m.Planet.UserID == userId)
                .Include(m => m.Action)
                .Where(m => m.Action != null && m.Action.Due_to < now)
                .Include(m => m.Caps)
                .OrderBy(m => m.Action.Due_to)
                .ToArray();

            Spaceship[] VesselList = context.Spaceships
                .Include(m => m.Planet)
                .Where(m => m.Planet.UserID == userId)
                .Include(m => m.Action)
                .Where(m => m.Action != null && m.Action.Due_to < now)
                .Include(m => m.Caps)
                .Include(m => m.Action.Target)
                .Include(m => m.Action.Target.Defenses)
                .OrderBy(m => m.Action.Due_to)
                .ToArray();

            IActionHolder[] alliedAction = new IActionHolder[Minelist.Length + DefenseList.Length + SolarList.Length + VesselList.Length];
            Minelist.CopyTo(alliedAction, 0);
            DefenseList.CopyTo(alliedAction, Minelist.Length);
            SolarList.CopyTo(alliedAction, Minelist.Length + DefenseList.Length);
            VesselList.CopyTo(alliedAction, Minelist.Length + DefenseList.Length + SolarList.Length);

            Array.Sort(alliedAction, IActionHolderSort);

            Spaceship[] ennemyVessels = context.Spaceships
                .Include(m => m.Action)
                .Where(m => m.Action != null)
                .Include(m => m.Action.Target)
                .Where(m => m.Action.Target.UserID == userId)
                .Where(m => m.Action.Due_to < now)
                .Include(m => m.Planet)
                .Include(m => m.Action.Target.Defenses)
                .OrderBy(m => m.Action.Due_to)
                .ToArray();

            int indA = 0;
            int indE = 0;
            while (indA < alliedAction.Length && indE < ennemyVessels.Length)
            {
                if (alliedAction[indA].Action.Due_to < ennemyVessels[indE].Action.Due_to)
                {
                    ResolveAction(context, alliedAction[indA], ennemyVessels[indE].Action.Due_to);
                    if (alliedAction[indA].Action != null && alliedAction[indA].Action.Due_to > ennemyVessels[indE].Action.Due_to)
                    {
                        Array.Sort(alliedAction, IActionHolderSort);
                    }
                    else
                    {
                        indA++;
                    }
                }
                else
                {
                    HandleTemoralActionForUserUntil(context, ennemyVessels[indE].Planet.UserID, ennemyVessels[indE].Action.Due_to);
                    ResolveAction(context, ennemyVessels[indE], now.Value);
                    indE++;
                }
            }
            while (indA < alliedAction.Length)
            {
                ResolveAction(context, alliedAction[indA], now.Value);
                indA++;
            }
            while (indE < ennemyVessels.Length)
            {
                HandleTemoralActionForUserUntil(context, ennemyVessels[indE].Planet.UserID, ennemyVessels[indE].Action.Due_to);
                ResolveAction(context, ennemyVessels[indE], now.Value);
                indE++;
            }

        }
    }
}
