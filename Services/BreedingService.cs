using System;
using System.Collections.Generic;
using System.Linq;
using ZooVillage.Models.Interfaces.Base;

namespace ZooVillage.Services
{
    public class BreedingService
    {
        private readonly Random _random = new Random();
        private const int MinBreedingIntervalDays = 180;
        private const int MaxBreedingIntervalDays = 210;
        private readonly Dictionary<Guid, int> _lastBirthDay = new Dictionary<Guid, int>();
        public int CurrentDay { get; private set; } = 0;

        public List<IJuvenile> TryBreed(IEnumerable<IAnimal> farm)
        {
            CurrentDay++;
            var newborns = new List<IJuvenile>();

            var females = farm.OfType<IFemaleBreeder>()
                .Where(f => f.CanBreed)
                .ToList();

            foreach (var female in females)
            {
                if (!CanGiveBirthNow(female))
                    continue;

                var male = FindSuitableMale(farm, female);

                if (male != null)
                {
                    if (_random.Next(100) < 30)
                    {
                        var newborn = female.GiveBirth(male);
                        if (newborn != null)
                        {
                            newborns.Add(newborn);
                            _lastBirthDay[female.Id] = CurrentDay;
                        }
                    }
                }
            }

            return newborns;
        }

        private bool CanGiveBirthNow(IFemaleBreeder female)
        {
            if (!_lastBirthDay.ContainsKey(female.Id))
                return true;

            int daysSinceLastBirth = CurrentDay - _lastBirthDay[female.Id];
            int requiredInterval = _random.Next(MinBreedingIntervalDays, MaxBreedingIntervalDays + 1);

            return daysSinceLastBirth >= requiredInterval;
        }

        private IMaleBreeder FindSuitableMale(IEnumerable<IAnimal> farm, IFemaleBreeder female)
        {
            var suitableMales = farm.OfType<IMaleBreeder>()
                .Where(m => m.CanImpregnate && m.Species == female.Species)
                .ToList();

            if (suitableMales.Count == 0)
                return null;

            return suitableMales[_random.Next(suitableMales.Count)];
        }
    }
}