using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ZooVillage.Views.Popups
{
    public partial class StorePopupView : UserControl
    {
        public event Action<StoreItem>? OnItemBought;
        public event Action<StoreItem>? OnItemSold;

        public StorePopupView()
        {
            InitializeComponent();
            InitializeStore();
        }

        private void InitializeStore()
        {
            var buyItems = new ObservableCollection<StoreItem>
            {
                new StoreItem { ItemName = "🐔 Курица", Price = 100, Available = 5 },
                new StoreItem { ItemName = "🐑 Овца",   Price = 150, Available = 3 },
                new StoreItem { ItemName = "🐐 Коза",   Price = 120, Available = 4 },
                new StoreItem { ItemName = "🐄 Корова", Price = 300, Available = 2 },
                new StoreItem { ItemName = "🐏 Баран",  Price = 180, Available = 1 },
            };

            var sellItems = new ObservableCollection<StoreItem>
            {
                new StoreItem { ItemName = "🥚 Яйцо",  SellPrice = 20, OwnCount = 15 },
                new StoreItem { ItemName = "🥛 Молоко", SellPrice = 50, OwnCount = 8  },
                new StoreItem { ItemName = "🧶 Шерсть", SellPrice = 40, OwnCount = 5  },
                new StoreItem { ItemName = "🌾 Зерно",  SellPrice = 30, OwnCount = 20 },
            };

            BuyItemsGrid.ItemsSource = buyItems;
            SellItemsGrid.ItemsSource = sellItems;

            BuyMoneyDisplay.Text = "1500";
            SellMoneyDisplay.Text = "1500";
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item })
                OnItemBought?.Invoke(item);
        }

        private void SellButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item })
                OnItemSold?.Invoke(item);
        }
    }

    public class StoreItem
    {
        public string ItemName { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Available { get; set; }
        public int SellPrice { get; set; }
        public int OwnCount { get; set; }
    }
}
