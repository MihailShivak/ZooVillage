namespace ZooVillage.Models.Products
{
    public class StoreItem : Product
    {
        public string ItemName { get; set; } = string.Empty;
        public override string Name => ItemName;
        public override int Price { get; set; }
        public int Available { get; set; }
        public int SellPrice { get; set; }
        public int OwnCount { get; set; }

        public static bool operator ==(StoreItem? a, StoreItem? b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Price == b.Price;
        }

        public static bool operator !=(StoreItem? a, StoreItem? b) => !(a == b);
        public static bool operator <(StoreItem a, StoreItem b) => a.Price < b.Price;
        public static bool operator >(StoreItem a, StoreItem b) => a.Price > b.Price;
        public static bool operator <=(StoreItem a, StoreItem b) => a.Price <= b.Price;
        public static bool operator >=(StoreItem a, StoreItem b) => a.Price >= b.Price;

        public override bool Equals(object? obj) => obj is StoreItem other && this == other;
        public override int GetHashCode() => Price.GetHashCode();
        public override string ToString() => $"{ItemName} [{Price} руб.]";
    }
}
