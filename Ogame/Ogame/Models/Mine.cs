using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogame.Models
{
    public class Mine : IActionHolder
    {
        public enum Ressources
        {
            Deuterium, Cristal, Metal
        }

        public int MineID { get; set; }
        public int PlanetID { get; set; }
        public int CapsID { get; set; }
        public int? ActionID { get; set; }
        public int Level { get; set; }
        public Ressources Ressource { get; set; }
        public float Collect_rate;

        public Planet Planet { get; set; }
        public Caps Caps { get; set; }
        public TemporalAction Action { get; set; }

    }
}
