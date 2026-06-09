using ZooVillage.Models.Enums;
using ZooVillage.Models.Interfaces.Groups;

namespace ZooVillage.Models.Base
{
    public abstract class Bird : AnimalBase, IBird
    {
        protected Bird(string name, Gender gender, double basePrice, int age = 0)
            : base(name, gender, basePrice, age) { }
    }
}