using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Ogame.Models;

namespace Ogame.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Users.Any()) {
                TemporalActionResolver.HandleTemoralActionForUserUntil(context, context.Users.First().Id);
                return;
            }

            var users = new User[] {
                new User {UserName="admin", Score=99999999, Email="test.admin@epita.fr'=", IsAdmin=true},
                new User {UserName="user1", Score=99999999, Email="test.user1@epita.fr'=", IsAdmin=false},
                new User {UserName="user2", Score=99999999, Email="test.user2@epita.fr'=", IsAdmin=false},
                new User {UserName="user3", Score=99999999, Email="test.user3@epita.fr'=", IsAdmin=false},
                new User {UserName="user4", Score=99999999, Email="test.user4@epita.fr'=", IsAdmin=false},
                new User {UserName="user5", Score=99999999, Email="test.user5@epita.fr'=", IsAdmin=false},
                new User {UserName="user6", Score=99999999, Email="test.user6@epita.fr'=", IsAdmin=false},
                new User {UserName="user7", Score=99999999, Email="test.user7@epita.fr'=", IsAdmin=false},
                new User {UserName="user8", Score=99999999, Email="test.user8@epita.fr'=", IsAdmin=false},
                new User {UserName="user9", Score=99999999, Email="test.user9@epita.fr'=", IsAdmin=false},
                new User {UserName="user10", Score=99999999, Email="test.user10@epita.fr'=", IsAdmin=false},
            };
            foreach (var user in users)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();

            var caps = new Caps[]
            {
                new Caps { Cristal_cap = 100, Deuterium_cap = 100, Energy_cap = 100, Growth_factor = 1.1f, Metal_cap = 100, Repair_factor = 1 },
                new Caps { Cristal_cap = 75, Deuterium_cap = 75, Energy_cap = 75, Growth_factor = 1.2f, Metal_cap = 75, Repair_factor = 1.1f },
                new Caps { Cristal_cap = 200, Deuterium_cap = 200, Energy_cap = 200, Growth_factor = 1.3f, Metal_cap = 200, Repair_factor = 1.2f },
                new Caps { Cristal_cap = 1000, Deuterium_cap = 1000, Energy_cap = 1000, Growth_factor = 1.4f, Metal_cap = 1000, Repair_factor = 1.3f },
            };
            foreach (var cap in caps)
            {
                context.Caps.Add(cap);
            }
            context.SaveChanges();

            var planets = new Planet[]
            {
                new Planet { Name="Solaris", Cristal=50000, Deuterium=10000, Dist_to_star=10000, Metal=10000, Energy=100000, UserID=users[0].Id, X=0, Y=0 },
                PlanetRandomizer.GetRandomPlanet(20, -20),
                PlanetRandomizer.GetRandomPlanet(40, -40),
                PlanetRandomizer.GetRandomPlanet(60, -60),
                PlanetRandomizer.GetRandomPlanet(-60, 60),
                PlanetRandomizer.GetRandomPlanet(-60, -60),
                PlanetRandomizer.GetRandomPlanet(60, 60),
                PlanetRandomizer.GetRandomPlanet(-20, 20),
                PlanetRandomizer.GetRandomPlanet(-20, -20),
                PlanetRandomizer.GetRandomPlanet(20, 20),
                PlanetRandomizer.GetRandomPlanet(-40, 40),
                PlanetRandomizer.GetRandomPlanet(-40, -40),
                PlanetRandomizer.GetRandomPlanet(40, 40),

            };
            foreach (var planet in planets)
            {
                context.Planets.Add(planet);
            }
            context.SaveChanges();

            var actions = new TemporalAction[]
            {
                new TemporalAction { Due_to=Convert.ToDateTime("2019-05-12 19:00"), Type=TemporalAction.ActionType.Production },
                new TemporalAction { Due_to=Convert.ToDateTime("2019-05-12 20:00"), Type=TemporalAction.ActionType.Upgrade },
                new TemporalAction { Due_to=Convert.ToDateTime("2019-05-12 21:00"), TargetID=1, Type=TemporalAction.ActionType.Attack },
            };
            foreach (var action in actions)
            {
                context.Actions.Add(action);
            }
            context.SaveChanges();

            var spaceships = new Spaceship[]
            {
                new Spaceship { ActionID=3, CapsID=4, Level=5, PlanetID=2, Energy=500 }
            };
            foreach (var spaceship in spaceships)
            {
                context.Spaceships.Add(spaceship);
            }
            context.SaveChanges();

            var solarpanels = new SolarPanel[]
            {
                new SolarPanel { ActionID=1, PlanetID=1, Level=2, Collect_rate=10, CapsID=2}
            };
            foreach (var solarpanel in solarpanels)
            {
                context.SolarPanels.Add(solarpanel);
            }
            context.SaveChanges();

            var mines = new Mine[]
            {
                new Mine { ActionID=2, Level=3, CapsID=1, Ressource=Mine.Ressources.Cristal, PlanetID=1, Collect_rate=15 }
            };
            foreach (var mine in mines)
            {
                context.Mines.Add(mine);
            }
            context.SaveChanges();

            var defenses = new Defense[]
            {
                new Defense { CapsID=3, Energy=100, PlanetID=1, Level=2 }
            };
            foreach (var defense in defenses)
            {
                context.Defenses.Add(defense);
            }
            context.SaveChanges();

            TemporalActionResolver.HandleTemoralActionForUserUntil(context, users[0].Id);

        }
    }
}
