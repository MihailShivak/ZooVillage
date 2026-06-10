using ZooVillage.Models.Base;
using ZooVillage.Models.Enums;
using ZooVillage.Models.Interfaces.Base;
using ZooVillage.Models.Interfaces.Specific;

namespace ZooVillage.Models.Animals.Birds
{
    public class Chick : Bird, IJuvenile, IChick
    {
        public int DaysOld { get; private set; }
        public string Species => "Chick";

        public Chick(string name, Gender gender, IAnimal mother, int age = 0)
            : base(name, gender, 500, age)
        {
            DaysOld = age;
        }

        public override IAnimal TryMature()
        {
            if (DaysOld >= 60)
                return Gender == Gender.Female ? new Chicken(Name) : new Rooster(Name);
            return null;
        }

        protected override double GetAgeMultiplier() => 0.9;
    }
}
