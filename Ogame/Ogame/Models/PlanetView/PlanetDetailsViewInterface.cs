using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogame.Models.PlanetView
{
    public class PlanetDetailsViewInterface
    {
        public Planet _planet { get; set; }
        public User _user { get; set; }

        public PlanetDetailsViewInterface(Planet planet, User user)
        {
            _planet = planet;
            _user = user;
        }
    }
}
