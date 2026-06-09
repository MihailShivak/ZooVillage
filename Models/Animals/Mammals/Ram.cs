using ZooVillage.Models.Base;
using ZooVillage.Models.Enums;
using ZooVillage.Models.Interfaces.Base;
using ZooVillage.Models.Interfaces.Specific;

namespace ZooVillage.Models.Animals.Mammals
{
    public class Ram : Mammal, IRam
    {
        public double WoolPerYear { get; private set; }
        public string Species => "Goat";
        public bool CanImpregnate => Stage == GrowthStage.Adult;
        public bool CanBreed => CanImpregnate;

        public Ram(string name, int age = 100) : base(name, Gender.Male, 15000, age)
        {
            WoolPerYear = 5.0;
        }

        public void Shear()
        {
            Console.WriteLine($"{Name} острижен, получено {WoolPerYear} кг шерсти!");
        }

        protected override double GetAgeMultiplier() =>
            Stage == GrowthStage.Young ? 1.3 : (Stage == GrowthStage.Old ? 0.6 : 1.0);

        public override IAnimal TryMature() => null;
    }
}