using System;
namespace Ogame.Models
{
    public interface IholderWithProduction : IActionHolder
    {
        float Collect_rate { get; set; }
    }
}
