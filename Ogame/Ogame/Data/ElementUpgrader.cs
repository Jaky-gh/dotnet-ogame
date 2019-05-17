using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ogame.Models;

namespace Ogame.Data
{
    public class ElementUpgrader
    {
        private static bool UpdatePlanet(ApplicationDbContext context, Planet planet, ActionCost actionCost)
        {
            planet.Cristal -= actionCost.CristalCost;
            planet.Metal -= actionCost.MetalCost;
            planet.Deuterium -= actionCost.DeuteriumCost;
            planet.Energy -= actionCost.EnergyCost;

            try
            {
                context.Planets.Update(planet);
                context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private static bool UpdateTemporalAction(ApplicationDbContext context, TemporalAction action,
            ActionCost actionCost)
        {
            action.Due_to = DateTime.Now + actionCost.ActionTime;
            action.Type = TemporalAction.ActionType.Upgrade;

            try
            {
                context.Actions.Update(action);
                context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static bool UpgradeMine(ApplicationDbContext context, Mine mine)
        {
            ActionCost actionCost = ActionCost.UpgradeMineCost(mine);
            return (ActionCost.CanUpgrade(actionCost, mine.Planet)
                    && UpdatePlanet(context, mine.Planet, actionCost) 
                    && UpdateTemporalAction(context, mine.Action, actionCost));
        }

        public static bool UpgradeSolarPanel(ApplicationDbContext context, SolarPanel solarPanel)
        {
            ActionCost actionCost = ActionCost.UpgradeSolarPanelCost(solarPanel);
            return (ActionCost.CanUpgrade(actionCost, solarPanel.Planet)
                    && UpdatePlanet(context, solarPanel.Planet, actionCost)
                    && UpdateTemporalAction(context, solarPanel.Action, actionCost));
        }

        public static bool UpgradeDefense(ApplicationDbContext context, Defense defense)
        {
            ActionCost actionCost = ActionCost.UpgradeDefenseActionCost(defense);
            return (ActionCost.CanUpgrade(actionCost, defense.Planet)
                    && UpdatePlanet(context, defense.Planet, actionCost)
                    && UpdateTemporalAction(context, defense.Action, actionCost));
        }

        public static bool UpgradeSpaceship(ApplicationDbContext context, Spaceship spaceship)
        {
            ActionCost actionCost = ActionCost.UpgradeSpaceshipActionCost(spaceship);
            return (ActionCost.CanUpgrade(actionCost, spaceship.Planet)
                    && UpdatePlanet(context, spaceship.Planet, actionCost)
                    && UpdateTemporalAction(context, spaceship.Action, actionCost));
        }
    }
}
