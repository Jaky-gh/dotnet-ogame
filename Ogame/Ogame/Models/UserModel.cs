using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogame.Models
{
    public class UserModel
    {
        public int _id { get; set; }
        public int _points { get; set; }
        public string _seed { get; set; }
        public string _username { get; set; }
        public string _password { get; set; }
        public string _email { get; set; }
    }
}
