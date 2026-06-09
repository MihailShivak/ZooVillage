namespace ZooVillage.Models.Products
{
    public abstract class Product : IProduct
    {
        public abstract string Name { get; }
        public abstract int Price { get; set; }
        public virtual string Description => $"{Name} — {Price} руб.";
    }
}
