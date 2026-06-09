namespace ZooVillage.Models.Interfaces.Base
{
    public interface IFemaleBreeder : IBreeder
    {
        int BirthCount { get; }
        int MaxBirths { get; }
        IJuvenile GiveBirth(IBreeder father);
        void IncrementBirthCount();
    }
}
