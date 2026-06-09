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

            BuyList.ItemsSource = _buyItems;
            SellList.ItemsSource = _sellItems;
        }

        // ───── Покупка: +/- через перегрузку операторов ─────

        private void BuyPlus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item })
            {
                item++;          // вызов operator++
                UpdateBuyTotal();
            }
        }

        private void BuyMinus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item })
            {
                item--;          // вызов operator--
                UpdateBuyTotal();
            }
        }

        private void UpdateBuyTotal()
        {
            int total = 0;
            foreach (var item in _buyItems)
                total += item.Price * item.Quantity;
            BuyTotalText.Text = $"{total} руб.";
        }

        private void BuyConfirm_Click(object sender, RoutedEventArgs e)
        {
            int total = 0;
            var lines = new System.Text.StringBuilder();
            foreach (var item in _buyItems)
            {
                if (item.Quantity > 0)
                {
                    lines.AppendLine($"{item.ItemName}  ×{item.Quantity}  = {item.Price * item.Quantity} руб.");
                    total += item.Price * item.Quantity;
                }
            }

            if (total == 0) { MessageBox.Show("Выберите товар.", "Покупка"); return; }

            MessageBox.Show($"{lines}\nИтого: {total} руб.", "Подтверждение покупки",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // ───── Продажа: +/- через перегрузку операторов ─────

        private void SellPlus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item })
            {
                item++;
                UpdateSellTotal();
            }
        }

        private void SellMinus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item })
            {
                item--;
                UpdateSellTotal();
            }
        }

        private void UpdateSellTotal()
        {
            int total = 0;
            foreach (var item in _sellItems)
                total += item.SellPrice * item.Quantity;
            SellTotalText.Text = $"{total} руб.";
        }

        private void SellConfirm_Click(object sender, RoutedEventArgs e)
        {
            int total = 0;
            var lines = new System.Text.StringBuilder();
            foreach (var item in _sellItems)
            {
                if (item.Quantity > 0)
                {
                    lines.AppendLine($"{item.ItemName}  ×{item.Quantity}  = {item.SellPrice * item.Quantity} руб.");
                    total += item.SellPrice * item.Quantity;
                }
            }

            if (total == 0) { MessageBox.Show("Выберите товар.", "Продажа"); return; }

            MessageBox.Show($"{lines}\nИтого: {total} руб.", "Подтверждение продажи",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
    }
}
