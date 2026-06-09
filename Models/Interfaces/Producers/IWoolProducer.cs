namespace ZooVillage.Models.Interfaces.Producers
{
    public interface IWoolProducer
    {
        double WoolPerYear { get; }
        void Shear();
    }
}