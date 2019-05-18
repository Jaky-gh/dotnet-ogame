using System;
using Ogame.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;

namespace Ogame.Data
{
    public static class TemporalActionResolver
    {
        public static readonly TimeSpan CycleDuration = new TimeSpan(0, 0, 1, 0);


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
                        (actionHolder as Spaceship).Energy -= defense.Energy;
                        defense.Energy -= (actionHolder as Spaceship).Energy;
                        if (defense.Energy < 0)
                        {
                            defense.Energy = 0;
                        }
                        context.Defenses.Update(defense);
                        if ((actionHolder as Spaceship).Energy <= 0)
                        {
                            (actionHolder as Spaceship).Energy = 0;
                            break;
                        }
                    }
                    if ((actionHolder as Spaceship).Energy > 0)
                    {
                        if (actionHolder.Action.Target.UserID == null)
                        {
                            actionHolder.Planet.User.Planets.Add(actionHolder.Action.Target);
                            actionHolder.Planet.User.Score += 1;
                            context.Users.Update(actionHolder.Planet.User);
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
                    actionHolder.Action.Due_to = actionHolder.Action.Due_to.Add(-CycleDuration);
                    break;
                case TemporalAction.ActionType.Upgrade:
                    actionHolder.Level += 1;
                    if (actionHolder is IHolderWithProduction)
                    {
                        ((IHolderWithProduction)actionHolder).CollectRate *= 1.2f;
                    }
                    float mult = actionHolder.Caps.Growth_factor;
                    actionHolder.Caps.Cristal_cap *= mult;
                    actionHolder.Caps.Metal_cap *= mult;
                    actionHolder.Caps.Deuterium_cap *= mult;
                    actionHolder.Caps.Energy_cap *= mult;
                    actionHolder.Caps.Repair_factor *= mult;
                    actionHolder.Caps.Growth_factor *= 1.2f;
                    context.Caps.Update(actionHolder.Caps);
                    context.Update(actionHolder);
                    if (actionHolder is Mine || actionHolder is SolarPanel)
                    {
                        actionHolder.Action.Type = TemporalAction.ActionType.Production;
                        actionHolder.Action.Due_to = actionHolder.Action.Due_to.Add(CycleDuration);
                        context.Actions.Update(actionHolder.Action);
                    }
                    break;
                case TemporalAction.ActionType.Idle:
                    if (actionHolder is IHolderWithEnergy)
                    {
                        TimeSpan interval = until - actionHolder.Action.Due_to;
                        int numCycle = (int)(interval / CycleDuration);
                        if (numCycle > 0)
                        {
                            float energy = MathF.Min(MathF.Min(
                                numCycle * actionHolder.Caps.Repair_factor
                                , actionHolder.Planet.Energy)
                                , actionHolder.Caps.Energy_cap - ((IHolderWithEnergy)actionHolder).Energy);
                            ((IHolderWithEnergy)actionHolder).Energy += energy;
                            actionHolder.Planet.Energy -= energy;
                            context.Update(actionHolder);
                            context.Planets.Update(actionHolder.Planet);
                        }
                    }
                    break;
            }
            if (actionHolder is Mine || actionHolder is SolarPanel)
            {
                TimeSpan interval = until - actionHolder.Action.Due_to;
                int numCycle = (int)(interval / CycleDuration);
                if (numCycle > 0)
                {
                    float produce = numCycle;
                    if (actionHolder is Mine)
                    {
                        produce *= ((Mine)actionHolder).CollectRate;
                        switch (((Mine)actionHolder).Ressource)
                        {
                            case Mine.Ressources.Cristal:
                                if (((Mine)actionHolder).Planet.Cristal + produce > ((Mine)actionHolder).Caps.Cristal_cap)
                                {
                                    ((Mine)actionHolder).Planet.Cristal = ((Mine)actionHolder).Caps.Cristal_cap;
                                }
                                else
                                {
                                    ((Mine)actionHolder).Planet.Cristal += produce;
                                }
                                break;
                            case Mine.Ressources.Metal:
                                if (((Mine)actionHolder).Planet.Metal + produce > ((Mine)actionHolder).Caps.Metal_cap)
                                {
                                    ((Mine)actionHolder).Planet.Metal = ((Mine)actionHolder).Caps.Metal_cap;
                                }
                                else
                                {
                                    ((Mine)actionHolder).Planet.Metal += produce;
                                }
                                break;
                            case Mine.Ressources.Deuterium:
                                if (((Mine)actionHolder).Planet.Deuterium + produce > ((Mine)actionHolder).Caps.Deuterium_cap)
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
                        produce *= ((SolarPanel)actionHolder).CollectRate;
                        if (((SolarPanel)actionHolder).Planet.Energy + produce > ((SolarPanel)actionHolder).Caps.Energy_cap)
                        {
                            ((SolarPanel)actionHolder).Planet.Energy = ((SolarPanel)actionHolder).Caps.Energy_cap;
                        }
                        else
                        {
                            ((SolarPanel)actionHolder).Planet.Energy += produce;
                        }
                    }
                    actionHolder.Action.Type = TemporalAction.ActionType.Production;
                    actionHolder.Action.Due_to = actionHolder.Action.Due_to.Add((numCycle + 1) * CycleDuration);
                    context.Actions.Update(actionHolder.Action);
                    context.Planets.Update(actionHolder.Planet);
                }
            }
            else
            {
                actionHolder.Action.Type = TemporalAction.ActionType.Idle;
                actionHolder.Action.Due_to = until.Add(CycleDuration);
                actionHolder.Action.TargetID = null;
                actionHolder.Action.Target = null;
            }
            context.SaveChanges();
        }

        public static void HandleTemoralActionForUserUntil(ApplicationDbContext context, string userId, DateTime? now = null)
        {
            if (now == null)
                now = DateTime.Now;
			
			if (userId == null)
				return;

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
                .Include(m => m.Planet.SolarPanels)
                .OrderBy(m => m.Action.Due_to)
                .ToArray();

            Spaceship[] VesselList = context.Spaceships
                .Include(m => m.Planet)
                .Where(m => m.Planet.UserID == userId)
                .Include(m => m.Planet.User)
                .Include(m => m.Planet.User.Planets)
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
				.Include(m => m.Caps)
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
