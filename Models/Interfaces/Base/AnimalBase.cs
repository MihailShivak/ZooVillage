using System;
using System.Collections.Generic;
using ZooVillage.Models.Composition;
using ZooVillage.Models.Enums;
using ZooVillage.Models.Interfaces.Base;

namespace ZooVillage.Models.Base
{
    public abstract class AnimalBase : IAnimal, IGrowable
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; protected set; }
        public int AgeInDays { get; protected set; }
        public Gender Gender { get; protected set; }
        public GrowthStage Stage { get; protected set; }

        public HealthRecord HealthInfo { get; private set; }
        public List<IAnimal> Children { get; } = new List<IAnimal>();
        public double BasePrice { get; protected set; }

        protected AnimalBase(string name, Gender gender, double basePrice, int initialAge = 0)
        {
            Name = name;
            Gender = gender;
            BasePrice = basePrice;
            AgeInDays = initialAge;
            HealthInfo = new HealthRecord(100);
            UpdateStage();
        }

        protected abstract double GetAgeMultiplier();

        public double CalculateCurrentPrice()
        {
            return BasePrice * HealthInfo.GetHealthMultiplier() * GetAgeMultiplier();
        }

        public virtual void AgeOneDay()
        {
            AgeInDays++;
            UpdateStage();

            if (Stage == GrowthStage.Old && new Random().Next(100) < 10)
            {
                HealthInfo.HealthPoints = Math.Max(0, HealthInfo.HealthPoints - 5);
            }
        }

        private void UpdateStage()
        {
            if (AgeInDays < 30) Stage = GrowthStage.Baby;
            else if (AgeInDays < 365) Stage = GrowthStage.Young;
            else if (AgeInDays < 1800) Stage = GrowthStage.Adult;
            else Stage = GrowthStage.Old;
        }

        public abstract IAnimal TryMature();
    }
}