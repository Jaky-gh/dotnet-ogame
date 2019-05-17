using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ogame.Models;

namespace Ogame.Data
{
    public class DefaultElementsGenerator
    {
        public static Caps CreateDefaultCaps(ApplicationDbContext context)
        {
            Caps caps = new Caps
            {
                Cristal_cap = 0,
                Deuterium_cap = 0,
                Growth_factor = 0,
                Metal_cap = 0,
                Energy_cap = 0,
                Repair_factor = 0,
            };
            try
            {
                context.Caps.Add(caps);
                context.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }
            return caps;
        }

        public static TemporalAction CreateDefaultAction(ApplicationDbContext context)
        {
            TemporalAction action = new TemporalAction
            {
                Target = null,
                TargetID = null,
                Type = TemporalAction.ActionType.Idle,
                Due_to = DateTime.Now
            };
            try
            {
                context.Actions.Add(action);
                context.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }
            return action;
        }


        public static Defense CreateDefaultDefense(ApplicationDbContext context, Planet planet)
        {
            TemporalAction action = CreateDefaultAction(context);
            Caps caps = CreateDefaultCaps(context);

            Defense defense = new Defense
            {
                Action = action,
                ActionID = action.TemporalActionID,
                Planet = planet,
                PlanetID = planet.PlanetID,
                Caps = caps,
                CapsID = caps.CapsID,
            };

            try
            {
                context.Defenses.Add(defense);
                context.SaveChanges();

                planet.Defenses.Add(defense);
                context.Planets.Update(planet);
                context.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }

            return defense;
        }

        public static Mine CreateDefaultMine(ApplicationDbContext context, Mine.Ressources mineRessources, Planet planet)
        {
            TemporalAction action = CreateDefaultAction(context);
            Caps caps = CreateDefaultCaps(context);

            Mine mine = new Mine
            {
                Action = action,
                ActionID = action.TemporalActionID,
                Caps = caps,
                CapsID = caps.CapsID,
                Level = 0,
                Ressource = mineRessources,
                Planet = planet,
                PlanetID = planet.PlanetID
            };

            try
            {
                context.Mines.Add(mine);
                context.SaveChanges();

                planet.Mines.Add(mine);
                context.Planets.Update(planet);
                context.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }
            return mine;
        }

        public static SolarPanel CreateDefaultSolarPanel(ApplicationDbContext context, Planet planet)
        {
            TemporalAction action = CreateDefaultAction(context);
            Caps caps = CreateDefaultCaps(context);

            SolarPanel solarPanel = new SolarPanel
            {
                Action = action,
                ActionID = action.TemporalActionID,
                Caps = caps,
                CapsID = caps.CapsID,
                Level = 0,
                Collect_rate = 0,
                Planet = planet,
                PlanetID = planet.PlanetID,
            };

            try
            {
                context.SolarPanels.Add(solarPanel);
                context.SaveChanges();

                planet.SolarPanels.Add(solarPanel);
                context.Planets.Update(planet);
                context.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }
            return solarPanel;
        }

        public static Spaceship CreateDefaultSpaceship(ApplicationDbContext context, Planet planet)
        {
            TemporalAction action = CreateDefaultAction(context);
            Caps caps = CreateDefaultCaps(context);

            Spaceship spaceship = new Spaceship
            {
                Level = 0,
                Energy = 0,
                Action = action,
                ActionID = action.TemporalActionID,
                Caps = caps,
                CapsID = caps.CapsID,
                Planet = planet,
                PlanetID = planet.PlanetID,
            };

            try
            {
                context.Spaceships.Add(spaceship);
                context.SaveChanges();

                planet.Spaceships.Add(spaceship);
                context.Planets.Update(planet);
                context.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }
            return spaceship;
        }
    }
}
