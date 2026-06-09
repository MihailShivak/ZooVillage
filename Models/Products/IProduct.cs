namespace ZooVillage.Models.Products
{
    public interface IProduct
    {
        string Name { get; }
        int Price { get; }
        string Description { get; }
    }
}
