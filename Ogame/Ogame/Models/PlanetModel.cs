using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogame.Models
{
    public class PlanetModel
    {
        public int _id { get; set; }
        public int _ownerId { get; set; }
        public int _distToStar { get; set; }
        public int _x { get; set; }
        public int _y { get; set; }
        public int _metal { get; set; }
        public int _cristal { get; set; }
        public int _deuterium { get; set; }
        public int _energy { get; set; }
        public string _name { get; set; }
    }
}
