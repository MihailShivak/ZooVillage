using System;
using System.Windows.Media.Animation;
using ZooVillage.Models.Base;
using ZooVillage.Models.Enums;
using ZooVillage.Models.Interfaces.Base;
using ZooVillage.Models.Interfaces.Specific;

namespace ZooVillage.Models.Animals.Birds
{
    public class Chicken : Bird, IChicken
    {
        public int EggsPerWeek { get; private set; }
        public int BirthCount { get; private set; } = 0;
        public int MaxBirths => 3;
        public string Species => "Chicken";

        public bool CanBreed =>
            Stage == GrowthStage.Adult &&
            BirthCount < MaxBirths &&
            HealthInfo.HealthPoints > 50;

        public Chicken(string name, int age = 60) : base(name, Gender.Female, 2000, age)
        {
            EggsPerWeek = 5;
        }

        public void LayEgg()
        {
            Console.WriteLine($"{Name} снесла яйцо!");
        }

        public IJuvenile GiveBirth(IBreeder father)
        {
            if (!CanBreed) return null;

            var gender = new Random().Next(2) == 0 ? Gender.Female : Gender.Male;
            var chick = new Chick($"Цыпленок от {Name}", gender, this);

            this.Children.Add(chick);
            if (father is IAnimal fatherAnimal)
            {
                fatherAnimal.Children.Add(chick);
            }

            IncrementBirthCount();
            return chick;
        }

        public void IncrementBirthCount()
        {
            BirthCount++;
        }

        protected override double GetAgeMultiplier() =>
            Stage == GrowthStage.Old ? 0.5 : 1.0;

        public override IAnimal TryMature() => null;
    }
}