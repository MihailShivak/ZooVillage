using System.ComponentModel;

namespace ZooVillage.Models.Products
{
    public class StoreItem : Product, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int _quantity;

        public string ItemName { get; set; } = string.Empty;
        public override string Name => ItemName;
        public override int Price { get; set; }
        public int Available { get; set; }
        public int SellPrice { get; set; }
        public int OwnCount { get; set; }

        // Количество выбранное для покупки/продажи
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Quantity)));
            }
        }

        // ++ увеличивает количество (но не больше Available/OwnCount)
        public static StoreItem operator ++(StoreItem item)
        {
            int max = item.SellPrice > 0 ? item.OwnCount : item.Available;
            if (item.Quantity < max)
                item.Quantity++;
            return item;
        }

        // -- уменьшает количество (но не меньше 0)
        public static StoreItem operator --(StoreItem item)
        {
            if (item.Quantity > 0)
                item.Quantity--;
            return item;
        }

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
