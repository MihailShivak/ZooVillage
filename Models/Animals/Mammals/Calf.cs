using ZooVillage.Models.Base;
using ZooVillage.Models.Enums;
using ZooVillage.Models.Interfaces.Base;
using ZooVillage.Models.Interfaces.Specific;

namespace ZooVillage.Models.Animals.Mammals
{
    public class Calf : Mammal, IJuvenile, ICalf
    {
        public int DaysOld { get; private set; }
        public string Species => "Calf";

        public Calf(string name, Gender gender, IAnimal mother, int age = 0)
            : base(name, gender, 15000, age)
        {
            DaysOld = age;
        }

        public override IAnimal TryMature()
        {
            if (DaysOld >= 365)
                return new Cow(Name);
            return null;
        }

        protected override double GetAgeMultiplier() => 0.8;
    }
}
