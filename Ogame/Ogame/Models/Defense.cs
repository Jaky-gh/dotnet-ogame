﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogame.Models
{
    public class Defense : IHolderWithEnergy
    {
        public int DefenseID { get; set; }
        public int PlanetID { get; set; }
        public Planet Planet { get; set; }
        public int Level { get; set; }
        public float Energy { get; set; }
        public int CapsID { get; set; }
        public int? ActionID { get; set; }

        public Caps Caps { get; set; }
        public TemporalAction Action { get; set; }
    }
}
