using System;
using ZooVillage.Models.Base;
using ZooVillage.Models.Enums;
using ZooVillage.Models.Interfaces.Base;
using ZooVillage.Models.Interfaces.Specific;

namespace ZooVillage.Models.Animals.Mammals
{
    public class Cow : Mammal, ICow
    {
        public double DailyMilkYield { get; private set; }
        public int BirthCount { get; private set; } = 0;
        public int MaxBirths => 3;
        public string Species => "Cow";

        public bool CanBreed =>
            Stage == GrowthStage.Adult &&
            BirthCount < MaxBirths &&
            HealthInfo.HealthPoints > 50;

        public Cow(string name, int age = 365) : base(name, Gender.Female, 50000, age)
        {
            DailyMilkYield = 20.0;
        }

        public void Milk()
        {
            Console.WriteLine($"{Name} дает {DailyMilkYield} литров молока!");
        }

        public IJuvenile GiveBirth(IBreeder father)
        {
            if (!CanBreed) return null;

            var gender = new Random().Next(2) == 0 ? Gender.Female : Gender.Male;
            var calf = new Calf($"Теленок от {Name}", gender, this);

            this.Children.Add(calf);
            if (father is IAnimal fatherAnimal)
            {
                fatherAnimal.Children.Add(calf);
            }

            IncrementBirthCount();
            return calf;
        }

        public void IncrementBirthCount()
        {
            BirthCount++;
        }

        protected override double GetAgeMultiplier() =>
            Stage == GrowthStage.Young ? 1.2 : (Stage == GrowthStage.Old ? 0.6 : 1.0);

        public override IAnimal TryMature() => null;
    }
}