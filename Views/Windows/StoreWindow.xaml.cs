using System.Windows;
using System.Windows.Controls;
using ZooVillage.Models.Collections;
using ZooVillage.Models.Products;

namespace ZooVillage.Views.Windows
{
    public partial class StoreWindow : BaseWindow
    {
        public override string WindowTitle => "Магазин";

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
            _buyItems.Add(new StoreItem { ItemName = "🐔 Курица", Price = 100, Available = 5 });
            _buyItems.Add(new StoreItem { ItemName = "🐑 Овца",   Price = 150, Available = 3 });
            _buyItems.Add(new StoreItem { ItemName = "🐐 Коза",   Price = 120, Available = 4 });
            _buyItems.Add(new StoreItem { ItemName = "🐄 Корова", Price = 300, Available = 2 });
            _buyItems.Add(new StoreItem { ItemName = "🐏 Баран",  Price = 180, Available = 1 });

            _sellItems.Add(new StoreItem { ItemName = "🥚 Яйцо",  SellPrice = 20, OwnCount = 15 });
            _sellItems.Add(new StoreItem { ItemName = "🥛 Молоко", SellPrice = 50, OwnCount = 8  });
            _sellItems.Add(new StoreItem { ItemName = "🧶 Шерсть", SellPrice = 40, OwnCount = 5  });
            _sellItems.Add(new StoreItem { ItemName = "🌾 Зерно",  SellPrice = 30, OwnCount = 20 });

            BuyItemsGrid.ItemsSource = _buyItems;
            SellItemsGrid.ItemsSource = _sellItems;

            BuyMoneyDisplay.Text = "1500";
            SellMoneyDisplay.Text = "1500";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item })
                MessageBox.Show($"Вы купили: {item.ItemName} за {item.Price} руб.", "Покупка",
                                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SellButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item })
                MessageBox.Show($"Вы продали: {item.ItemName} за {item.SellPrice} руб.", "Продажа",
                                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
