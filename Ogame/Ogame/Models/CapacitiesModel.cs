using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogame.Models
{
    public class CapacitiesModel
    {
        public int _id { get; set; }
        public int _metalCap { get; set; }
        public int _cristalCap { get; set; }
        public int _deuteriumCap { get; set; }
        public int _energyCap { get; set; }
        public float _growthFactor { get; set; }
        public float _repairFactor { get; set; }
    }
}
