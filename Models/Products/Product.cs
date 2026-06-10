namespace ZooVillage.Models.Products
{
    public class Product : IProduct
    {
        public virtual string Name { get; set; } = string.Empty;
        public virtual int Price { get; set; }
        public virtual string Description => $"{Name} — {Price} руб.";

        // Дополнительные свойства для работы с инвентарем
        public ProductType Type { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; } = string.Empty; // Единица измерения (шт, л, кг)

        public Product() { }

        public Product(ProductType type, int quantity, string unit)
        {
            Type = type;
            Quantity = quantity;
            Unit = unit;

            Name = type switch
            {
                ProductType.Egg => "Яйцо",
                ProductType.Milk => "Молоко",
                ProductType.Wool => "Шерсть",
                _ => "Неизвестно"
            };

            Price = type switch
            {
                ProductType.Egg => 20,
                ProductType.Milk => 50,
                ProductType.Wool => 40,
                _ => 0
            };
        }
    }
}
