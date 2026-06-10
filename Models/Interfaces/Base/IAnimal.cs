using System;
using System.Collections.Generic;
using ZooVillage.Models.Composition;
using ZooVillage.Models.Enums;

namespace ZooVillage.Models.Interfaces.Base
{
<<<<<<< HEAD
    internal interface IAnimal
=======
    public interface IAnimal
>>>>>>> 6fd75df9a0d0030ab360395fbae181d13d637ff4
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
