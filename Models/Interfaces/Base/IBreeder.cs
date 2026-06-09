namespace ZooVillage.Models.Interfaces.Base
{
    public interface IBreeder : IAnimal
    {
        bool CanBreed { get; }
        string Species { get; }
    }
}