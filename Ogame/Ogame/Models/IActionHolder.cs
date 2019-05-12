using System;
namespace Ogame.Models
{
    public interface IActionHolder
    {
        int? ActionID { get; set; }
        TemporalAction Action { get; set; }
        Planet Planet { get; set; }
        int PlanetID { get; set; }
        int Level { get; set; }
        int CapsID { get; set; }
        Caps Caps { get; set; }
    }
}
