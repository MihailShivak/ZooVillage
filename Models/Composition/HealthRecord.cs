namespace ZooVillage.Models.Composition
{

    // Медицинская карта животного (Композиция - не живет без животного)
    public class HealthRecord
    {
        public int HealthPoints { get; set; }

        public HealthRecord(int initialHealth = 100)
        {
            HealthPoints = initialHealth;
        }

        // Множитель цены от здоровья (0.0 до 1.0)
        public double GetHealthMultiplier()
        {
            return HealthPoints / 100.0;
        }
    }
}