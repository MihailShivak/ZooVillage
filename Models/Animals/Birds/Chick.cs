using System;
using ZooVillage.Models.Base;
using ZooVillage.Models.Enums;
using ZooVillage.Models.Interfaces.Base;
using ZooVillage.Models.Interfaces.Specific;

namespace ZooVillage.Models.Animals.Birds
{
    public class Chick : Bird, IChick
    {
        public IAnimal Parent { get; }
        public int DaysSinceBirth => AgeInDays;

        public Chick(string name, Gender gender, IAnimal parent)
            : base(name, gender, 500, 0)
        {
            Parent = parent;
        }

        protected override double GetAgeMultiplier() => 1.4;

        public override IAnimal TryMature()
        {
            if (AgeInDays >= 60)
            {
                if (Gender == Gender.Female)
                {
                    var chicken = new Chicken(this.Name, this.AgeInDays);
                    chicken.HealthInfo.HealthPoints = this.HealthInfo.HealthPoints;
                    return chicken;
                }
                else
                {
                    var rooster = new Rooster(this.Name, this.AgeInDays);
                    rooster.HealthInfo.HealthPoints = this.HealthInfo.HealthPoints;
                    return rooster;
                }
            }
            return null;
        }
    }
}