using ZooVillage.Models.Base;
using ZooVillage.Models.Enums;
using ZooVillage.Models.Interfaces.Base;
using ZooVillage.Models.Interfaces.Specific;

namespace ZooVillage.Models.Animals.Birds
{
    public class Rooster : Bird, IRooster
    {
        public string Species => "Chicken";
        public bool CanImpregnate => Stage == GrowthStage.Adult;
        public bool CanBreed => CanImpregnate;

        public Rooster(string name, int age = 60) : base(name, Gender.Male, 1800, age) { }

        protected override double GetAgeMultiplier() =>
            Stage == GrowthStage.Old ? 0.5 : 1.0;

        public override IAnimal TryMature() => null;
    }
}