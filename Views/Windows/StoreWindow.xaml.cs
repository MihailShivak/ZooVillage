using System.Windows;
using System.Windows.Controls;
using ZooVillage.Models.Collections;
using ZooVillage.Models.Products;

namespace ZooVillage.Views.Windows
{
    public partial class StoreWindow : BaseWindow
    {
        public override string WindowTitle => "Магазин";

        public event Action<string>? OnPurchaseSuccess;
        public event Action<string>? OnInsufficientFunds;

        private readonly StoreItemCollection _buyItems = new();
        private readonly StoreItemCollection _sellItems = new();

        public StoreWindow()
        {
            InitializeComponent();
            Title = WindowTitle;
            InitializeStore();
        }

        private void InitializeStore()
        {
            _buyItems.Add(new StoreItem { ItemName = "Курица", Price = 100, Available = 5, ImagePath = "/Assets/Animals/chicken.png" });
            _buyItems.Add(new StoreItem { ItemName = "Овца",   Price = 150, Available = 3, ImagePath = "/Assets/Animals/sheep.png"   });
            _buyItems.Add(new StoreItem { ItemName = "Корова", Price = 300, Available = 2, ImagePath = "/Assets/Animals/cow.png"     });
            _buyItems.Add(new StoreItem { ItemName = "Баран",  Price = 180, Available = 1, ImagePath = "/Assets/Animals/sheep.png"   });

            _sellItems.Add(new StoreItem { ItemName = "Яйцо",  SellPrice = 20, OwnCount = 15, ImagePath = "/Assets/Items/egg.png"  });
            _sellItems.Add(new StoreItem { ItemName = "Молоко", SellPrice = 50, OwnCount = 8,  ImagePath = "/Assets/Items/milk.png" });
            _sellItems.Add(new StoreItem { ItemName = "Шерсть", SellPrice = 40, OwnCount = 5,  ImagePath = "/Assets/Items/wool.png" });
            _sellItems.Add(new StoreItem { ItemName = "Зерно",  SellPrice = 30, OwnCount = 20, ImagePath = "/Assets/Items/hay.png"  });

            BuyList.ItemsSource = _buyItems;
            SellList.ItemsSource = _sellItems;
        }

        private void BuyPlus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item }) { item++; UpdateBuyTotal(); }
        }

        private void BuyMinus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item }) { item--; UpdateBuyTotal(); }
        }

        private void UpdateBuyTotal()
        {
            int total = 0;
            foreach (var item in _buyItems) total += item.Price * item.Quantity;
            BuyTotalText.Text = $"{total} монет";
        }

        private void BuyConfirm_Click(object sender, RoutedEventArgs e)
        {
            int total = 0;
            foreach (var item in _buyItems) total += item.Price * item.Quantity;
            if (total == 0) { OnInsufficientFunds?.Invoke("Выберите товар!"); return; }
            OnPurchaseSuccess?.Invoke("Успешная покупка!");
            Close();
        }

        private void SellPlus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item }) { item++; UpdateSellTotal(); }
        }

        private void SellMinus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item }) { item--; UpdateSellTotal(); }
        }

        private void UpdateSellTotal()
        {
            int total = 0;
            foreach (var item in _sellItems) total += item.SellPrice * item.Quantity;
            SellTotalText.Text = $"{total} монет";
        }

        private void SellConfirm_Click(object sender, RoutedEventArgs e)
        {
            int total = 0;
            foreach (var item in _sellItems) total += item.SellPrice * item.Quantity;
            if (total == 0) { OnInsufficientFunds?.Invoke("Выберите товар!"); return; }
            OnPurchaseSuccess?.Invoke("Продажа выполнена!");
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
    }
}
