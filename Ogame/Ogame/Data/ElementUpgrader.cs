using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ogame.Models;

namespace Ogame.Data
{
    public static class ElementUpgrader
    {
        private static bool AddSpaceship(ApplicationDbContext context, Planet planet, ActionCost actionCost)
        {
            planet.Deuterium -= actionCost.DeuteriumCost;
            context.Update(planet);
            context.SaveChanges();
            try
            {
                DefaultElementsGenerator.CreateDefaultSpaceship(context, planet);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        } 

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
            var response = (ActionCost.CanUpgrade(actionCost, mine.Planet)
                    && UpdatePlanet(context, mine.Planet, actionCost) 
                    && UpdateTemporalAction(context, mine.Action, actionCost));
            if (response)
            {
                mine.Planet.User.Score += 1;
                context.Users.Update(mine.Planet.User);
                context.SaveChanges();
            }
            return response;
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
            var response = (ActionCost.CanUpgrade(actionCost, defense.Planet)
                    && UpdatePlanet(context, defense.Planet, actionCost)
                    && UpdateTemporalAction(context, defense.Action, actionCost));
            if (response)
            {
                defense.Planet.User.Score += 1;
                context.Users.Update(defense.Planet.User);
                context.SaveChanges();
            }
            return response;
        }

        public static bool UpgradeSpaceship(ApplicationDbContext context, Spaceship spaceship)
        {
            ActionCost actionCost = ActionCost.UpgradeSpaceshipActionCost(spaceship);
            var response = (ActionCost.CanUpgrade(actionCost, spaceship.Planet)
                    && UpdatePlanet(context, spaceship.Planet, actionCost)
                    && UpdateTemporalAction(context, spaceship.Action, actionCost));
            if (response)
            {
                spaceship.Planet.User.Score += 1;
                context.Users.Update(spaceship.Planet.User);
                context.SaveChanges();
            }
            return response;
        }

        public static bool addSpaceship(ApplicationDbContext context, Planet planet)
        {
            ActionCost actionCost = ActionCost.createSpaceshipCost();
            var response = ActionCost.CanCreateSpaceship(actionCost, planet)
                && AddSpaceship(context, planet, actionCost);
            if (response)
            {
                planet.User.Score += 1;
                context.Users.Update(planet.User);
                context.SaveChanges();
            }
            return response;
        }
    }
}
