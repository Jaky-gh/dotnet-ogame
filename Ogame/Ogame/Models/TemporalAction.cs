using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogame.Models
{
    public class TemporalAction
    {
        public enum ActionType {
            Production, Upgrade, Attack
        }

        public int TemporalActionID { get; set; }
        public DateTime Due_to { get; set; }
        public ActionType Type { get; set; }
        public Planet Target { get; set; }
        public int? TargetID { get; set; }
    }
}
