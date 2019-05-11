using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Ogame.Models
{
    public class User : IdentityUser
    {
        public int Score { get; set; }
        public bool IsAdmin { get; set; }

        public ICollection<Planet> Planets { get; set; }
    }
}
