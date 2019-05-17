using System;
using Ogame.Models;
using System.Threading.Tasks;

namespace Ogame.Data
{
    public static class VesselAttackHandler
    {
        public async static Task<bool> AttackWithSpaceship(ApplicationDbContext context, Spaceship spaceship, int X, int Y)
        {
            ActionCost cost = ActionCost.AttackCost(spaceship, X, Y);
            if (!ActionCost.CanAttack(cost, spaceship))
            {
                return false;
            }

            TemporalAction temporalAction = spaceship.Action;

            Planet planet = await PlanetRandomizer.GetExistingOrRandomPlanet(context, X, Y);

            if (planet.PlanetID == 0)
            {
                context.Add(planet);
                context.SaveChanges();

                DefaultElementsGenerator.CreateDefaultSpaceship(context, planet);
                DefaultElementsGenerator.CreateDefaultMine(context, Mine.Ressources.Metal, planet);
                DefaultElementsGenerator.CreateDefaultMine(context, Mine.Ressources.Cristal, planet);
                DefaultElementsGenerator.CreateDefaultMine(context, Mine.Ressources.Deuterium, planet);
                DefaultElementsGenerator.CreateDefaultSolarPanel(context, planet);
                DefaultElementsGenerator.CreateDefaultDefense(context, planet);
            }
            spaceship.Energy -= cost.EnergyCost;
            context.Spaceships.Update(spaceship);
            
            temporalAction.Due_to = DateTime.Now.Add(cost.ActionTime);
            temporalAction.Type = TemporalAction.ActionType.Attack;
            temporalAction.TargetID = planet.PlanetID;

            context.Update(temporalAction);
            context.SaveChanges();
            return true;
        }
    }
}
