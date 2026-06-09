using ZooVillage.Models.Base;
using ZooVillage.Models.Enums;
using ZooVillage.Models.Interfaces.Base;
using ZooVillage.Models.Interfaces.Specific;

namespace ZooVillage.Models.Animals.Mammals
{
    public class Bull : Mammal, IBull
    {
        public string Species => "Cow";
        public bool CanImpregnate => Stage == GrowthStage.Adult;
        public bool CanBreed => CanImpregnate;

        public Bull(string name, int age = 365) : base(name, Gender.Male, 60000, age) { }

        protected override double GetAgeMultiplier() =>
            Stage == GrowthStage.Young ? 1.2 : (Stage == GrowthStage.Old ? 0.5 : 1.0);

        public override IAnimal TryMature() => null;
    }
}