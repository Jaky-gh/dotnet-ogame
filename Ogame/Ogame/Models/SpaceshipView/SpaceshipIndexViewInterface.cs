using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogame.Models.SpaceshipView
{
    public class SpaceshipIndexViewInterface
    {
        public List<Spaceship> _spaceship { get; set; }
        public User _user { get; set; }

        public SpaceshipIndexViewInterface(List<Spaceship> spaceship, User user)
        {
            _spaceship = spaceship;
            _user = user;
        }
    }
}
