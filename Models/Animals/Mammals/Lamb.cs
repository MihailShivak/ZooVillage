using System;
using ZooVillage.Models.Base;
using ZooVillage.Models.Enums;
using ZooVillage.Models.Interfaces.Base;
using ZooVillage.Models.Interfaces.Specific;

namespace ZooVillage.Models.Animals.Mammals
{
    public class Lamb : Mammal, ILamb
    {
        public IAnimal Parent { get; }
        public int DaysSinceBirth => AgeInDays;

        public Lamb(string name, Gender gender, IAnimal parent)
            : base(name, gender, 3000, 0)
        {
            Parent = parent;
        }

        protected override double GetAgeMultiplier() => 1.5;

        public override IAnimal TryMature()
        {
            if (AgeInDays >= 180)
            {
                var ram = new Ram(this.Name, this.AgeInDays);
                ram.HealthInfo.HealthPoints = this.HealthInfo.HealthPoints;
                return ram;
            }
            return null;
        }
    }
}