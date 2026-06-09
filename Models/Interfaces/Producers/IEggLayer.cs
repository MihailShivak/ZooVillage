namespace ZooVillage.Models.Interfaces.Producers
{
    public interface IEggLayer
    {
        int EggsPerWeek { get; }
        void LayEgg();
    }
}