using System;
using ZooVillage.Models.Base;
using ZooVillage.Models.Enums;
using ZooVillage.Models.Interfaces.Base;
using ZooVillage.Models.Interfaces.Specific;

namespace ZooVillage.Models.Animals.Mammals
{
    public class Kid : Mammal, IKid
    {
        public IAnimal Parent { get; }
        public int DaysSinceBirth => AgeInDays;

        public Kid(string name, Gender gender, IAnimal parent)
            : base(name, gender, 2000, 0)
        {
            Parent = parent;
        }

        protected override double GetAgeMultiplier() => 1.5;

        public override IAnimal TryMature()
        {
            if (AgeInDays >= 150)
            {
                var goat = new Goat(this.Name, this.Gender, this.AgeInDays);
                goat.HealthInfo.HealthPoints = this.HealthInfo.HealthPoints;
                return goat;
            }
            return null;
        }
    }
}