using System;
namespace Ogame.Models
{
    public interface IHolderWithProduction : IActionHolder
    {
        float Collect_rate { get; set; }
    }
}
