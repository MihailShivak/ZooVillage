namespace ZooVillage.Models.Interfaces.Base
{
    public interface IJuvenile : IAnimal, IGrowable
    {
        IAnimal Parent { get; }
        int DaysSinceBirth { get; }
    }
}