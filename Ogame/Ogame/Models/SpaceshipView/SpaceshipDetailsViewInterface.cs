using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogame.Models.SpaceshipView
{
    public class SpaceshipDetailsViewInterface
    {
        public Spaceship _spaceship { get; set; }
        public User _user { get; set; }

        public SpaceshipDetailsViewInterface(Spaceship spaceship, User user)
        {
            _spaceship = spaceship;
            _user = user;
        }
    }
}
