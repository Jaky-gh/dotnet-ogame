using System;
using Microsoft.EntityFrameworkCore;
using app.Models;
namespace app.Data
{
    public class PlanetContext : DbContext
    {
        public PlanetContext(DbContextOptions<PlanetContext> options) : base(options)
        {

        }

        public DbSet<Planet> Planet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Planet>().ToTable("Planet");
        }
    }
}
