using System;
using System.Security.Cryptography;
using ZooVillage.Models.Base;
using ZooVillage.Models.Enums;
using ZooVillage.Models.Interfaces.Base;
using ZooVillage.Models.Interfaces.Specific;

namespace ZooVillage.Models.Animals.Mammals
{
    public class Goat : Mammal, IGoat
    {
        public double DailyMilkYield { get; private set; }
        public int BirthCount { get; private set; } = 0;
        public int MaxBirths => 3;
        public string Species => "Goat";

        public bool CanBreed =>
            Stage == GrowthStage.Adult &&
            BirthCount < MaxBirths &&
            HealthInfo.HealthPoints > 50;

        public Goat(string name, Gender gender, int age = 100) : base(name, gender, 15000, age)
        {
            DailyMilkYield = 3.0;
        }

        public void Milk()
        {
            Console.WriteLine($"{Name} дает {DailyMilkYield} литров козьего молока!");
        }

        public IJuvenile GiveBirth(IBreeder father)
        {
            if (!CanBreed) return null;

            var gender = new Random().Next(2) == 0 ? Gender.Female : Gender.Male;
            var kid = new Kid($"Козленок от {Name}", gender, this);

            this.Children.Add(kid);
            if (father is IAnimal fatherAnimal)
            {
                fatherAnimal.Children.Add(kid);
            }

            IncrementBirthCount();
            return kid;
        }

        public void IncrementBirthCount()
        {
            BirthCount++;
        }

        protected override double GetAgeMultiplier() =>
            Stage == GrowthStage.Young ? 1.3 : (Stage == GrowthStage.Old ? 0.6 : 1.0);

        public override IAnimal TryMature() => null;
    }
}
