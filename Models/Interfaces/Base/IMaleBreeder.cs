namespace ZooVillage.Models.Interfaces.Base
{
    public interface IMaleBreeder : IBreeder
    {
        bool CanImpregnate { get; }
    }
}