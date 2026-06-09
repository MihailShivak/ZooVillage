using System;
using ZooVillage.Models.Base;
using ZooVillage.Models.Enums;
using ZooVillage.Models.Interfaces.Base;
using ZooVillage.Models.Interfaces.Specific;

namespace ZooVillage.Models.Animals.Mammals
{
    public class Calf : Mammal, ICalf
    {
        public IAnimal Parent { get; }
        public int DaysSinceBirth => AgeInDays;

        public Calf(string name, Gender gender, IAnimal parent)
            : base(name, gender, 5000, 0)
        {
            Parent = parent;
        }

        protected override double GetAgeMultiplier() => 1.5;

        public override IAnimal TryMature()
        {
            if (AgeInDays >= 365)
            {
                if (Gender == Gender.Female)
                {
                    var cow = new Cow(this.Name, this.AgeInDays);
                    cow.HealthInfo.HealthPoints = this.HealthInfo.HealthPoints;
                    return cow;
                }
                else
                {
                    var bull = new Bull(this.Name, this.AgeInDays);
                    bull.HealthInfo.HealthPoints = this.HealthInfo.HealthPoints;
                    return bull;
                }
            }
            return null;
        }
    }
}