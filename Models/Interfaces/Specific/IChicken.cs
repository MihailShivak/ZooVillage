using ZooVillage.Models.Interfaces.Base;
using ZooVillage.Models.Interfaces.Groups;
using ZooVillage.Models.Interfaces.Producers;

namespace ZooVillage.Models.Interfaces.Specific
{
    public interface IChicken : IBird, IEggLayer, IFemaleBreeder { }
}