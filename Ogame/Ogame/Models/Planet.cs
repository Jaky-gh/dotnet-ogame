using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogame.Models
{
    public class Planet
    {
        public int PlanetID { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public int Dist_to_star { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public float Metal { get; set; }
        public float Cristal { get; set; }
        public float Deuterium { get; set; }
        public float Energy { get; set; }

        public ICollection<Spaceship> Spaceships { get; set; }
        public ICollection<Defense> Defenses { get; set; }
        public ICollection<Mine> Mines { get; set; }
        public ICollection<SolarPanel> SolarPanels { get; set; }
        public ICollection<TemporalAction> TemporalActions { get; set; }
    }
}
