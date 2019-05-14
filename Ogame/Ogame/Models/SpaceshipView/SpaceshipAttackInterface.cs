using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogame.Models.SpaceshipView
{
    public class SpaceshipAttackInterface
    {
        public int _X { get; set; }
        public int _Y { get; set; }
        
        public SpaceshipAttackInterface()
        {
            _X = 0;
            _Y = 0;
        }

        public SpaceshipAttackInterface(int X, int Y)
        {
            _X = X;
            _Y = Y;
        }
    }
}
