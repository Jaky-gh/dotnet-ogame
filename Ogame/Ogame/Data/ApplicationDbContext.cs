using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ogame.Models;

namespace Ogame.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Caps> Caps { get; set; }
        public DbSet<Defense> Defenses { get; set; }
        public DbSet<Mine> Mines { get; set; }
        public DbSet<Spaceship> Spaceships { get; set; }
        public DbSet<Planet> Planets { get; set; }
        public DbSet<SolarPanel> SolarPanels { get; set; }
        public DbSet<TemporalAction> Actions { get; set; }
    }
}
