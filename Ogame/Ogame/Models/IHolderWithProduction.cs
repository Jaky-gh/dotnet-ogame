using System;
namespace Ogame.Models
{
    public interface IHolderWithProduction : IActionHolder
    {
        float CollectRate { get; set; }
    }
}
