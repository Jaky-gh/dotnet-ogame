using System;
namespace Ogame.Models
{
    public interface IHolderWithEnergy : IActionHolder
    {
        float Energy { get; set; }
    }
}
