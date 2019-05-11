﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogame.Models
{
    public class SolarPanelModel
    {
        public int _id { get; set; }
        public int _planetId { get; set; }
        public int _capsId { get; set; }
        public int _actionId { get; set; }
        public int _level { get; set; }
        public float _collectRate { get; set; }
    }
}