using System;
using System.Collections.Generic;
using ZooVillage.Models.Composition;
using ZooVillage.Models.Enums;

namespace ZooVillage.Models.Interfaces.Base
{
    public interface IAnimal
    {
        Guid Id { get; }
        string Name { get; }
        int AgeInDays { get; }
        Gender Gender { get; }
        GrowthStage Stage { get; }
        HealthRecord HealthInfo { get; }
        List<IAnimal> Children { get; }
        double CalculateCurrentPrice();
        void AgeOneDay();
    }
}
