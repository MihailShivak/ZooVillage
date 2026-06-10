using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ZooVillage.Models.Animals.Birds;
using ZooVillage.Models.Animals.Mammals;
using ZooVillage.Models.Collections;
using ZooVillage.Models.Interfaces.Base;
using ZooVillage.Models.Products;
using ZooVillage.ViewModels;

namespace ZooVillage.Views.Windows
{
    public partial class StoreWindow : BaseWindow
    {
        public override string WindowTitle => "Магазин";

        public event Action<string>? OnPurchaseSuccess;
        public event Action<string>? OnInsufficientFunds;

        private readonly StoreItemCollection _buyAnimals = new();
        private readonly StoreItemCollection _buyResources = new();
        private readonly ObservableCollection<StoreItem> _sellAnimals = new();
        private readonly ObservableCollection<StoreItem> _sellResources = new();
        private readonly FarmViewModel _viewModel;

        public StoreWindow(FarmViewModel viewModel)
        {
            InitializeComponent();
            Title = WindowTitle;
            _viewModel = viewModel;
            InitializeStore();
        }

        private void InitializeStore()
        {
            // ===== ПОКУПКА: ЖИВОТНЫЕ =====
            _buyAnimals.Add(new StoreItem { ItemName = "Курица", Price = 100, Available = 5, ImagePath = "/Assets/Animals/chicken.png" });
            _buyAnimals.Add(new StoreItem { ItemName = "Овца",   Price = 150, Available = 3, ImagePath = "/Assets/Animals/sheep.png"   });
            _buyAnimals.Add(new StoreItem { ItemName = "Корова", Price = 300, Available = 2, ImagePath = "/Assets/Animals/cow.png"     });
            _buyAnimals.Add(new StoreItem { ItemName = "Баран",  Price = 180, Available = 1, ImagePath = "/Assets/Animals/sheep.png"   });

            // ===== ПОКУПКА: СЫРЬЁ =====
            _buyResources.Add(new StoreItem { ItemName = "Яйцо",  Price = 20, Available = 100, ImagePath = "/Assets/Items/egg.png"  });
            _buyResources.Add(new StoreItem { ItemName = "Молоко", Price = 50, Available = 50, ImagePath = "/Assets/Items/milk.png" });
            _buyResources.Add(new StoreItem { ItemName = "Шерсть", Price = 40, Available = 50, ImagePath = "/Assets/Items/wool.png" });
            _buyResources.Add(new StoreItem { ItemName = "Зерно",  Price = 30, Available = 100, ImagePath = "/Assets/Items/hay.png"  });

            // ===== ПРОДАЖА: ЖИВОТНЫЕ =====
            foreach (var animal in _viewModel.Farm)
            {
                _sellAnimals.Add(new StoreItem
                {
                    ItemName = animal.Name,
                    SellPrice = (int)animal.CalculateCurrentPrice(),
                    OwnCount = 1,
                    ImagePath = GetAnimalImagePath(animal.GetType().Name)
                });
            }

            // ===== ПРОДАЖА: СЫРЬЁ =====
            foreach (var kvp in _viewModel.Inventory)
            {
                var imagePath = kvp.Key switch
                {
                    "Яйцо" => "/Assets/Items/egg.png",
                    "Молоко" => "/Assets/Items/milk.png",
                    "Шерсть" => "/Assets/Items/wool.png",
                    "Зерно" => "/Assets/Items/hay.png",
                    _ => "/Assets/Items/egg.png"
                };

                var sellPrice = kvp.Key switch
                {
                    "Яйцо" => 20,
                    "Молоко" => 50,
                    "Шерсть" => 40,
                    "Зерно" => 30,
                    _ => 0
                };

                _sellResources.Add(new StoreItem
                {
                    ItemName = kvp.Key,
                    SellPrice = sellPrice,
                    OwnCount = kvp.Value,
                    ImagePath = imagePath
                });
            }

            BuyAnimalList.ItemsSource = _buyAnimals;
            BuyResourceList.ItemsSource = _buyResources;
            SellAnimalList.ItemsSource = _sellAnimals;
            SellResourceList.ItemsSource = _sellResources;
        }

        private string GetAnimalImagePath(string animalType)
        {
            return animalType switch
            {
                "Cow" or "Bull" or "Calf" => "/Assets/Animals/cow.png",
                "Chicken" or "Rooster" or "Chick" => "/Assets/Animals/chicken.png",
                "Ram" or "Lamb" => "/Assets/Animals/sheep.png",
                _ => "/Assets/Animals/chicken.png"
            };
        }

        // ===== ПОКУПКА ЖИВОТНЫХ =====
        private void BuyAnimalPlus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item }) { item++; UpdateBuyAnimalTotal(); }
        }
        private void BuyAnimalMinus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item }) { item--; UpdateBuyAnimalTotal(); }
        }

        // ===== ПОКУПКА СЫРЬЯ =====
        private void BuyResourcePlus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item }) { item++; UpdateBuyResourceTotal(); }
        }
        private void BuyResourceMinus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item }) { item--; UpdateBuyResourceTotal(); }
        }
        private void UpdateBuyAnimalTotal()
        {
            int total = 0;
            foreach (var item in _buyAnimals) total += item.Price * item.Quantity;
            BuyAnimalTotalText.Text = $"{total} монет";
        }
        private void BuyAnimalConfirm_Click(object sender, RoutedEventArgs e)
        {
            int total = 0;
            foreach (var item in _buyAnimals) total += item.Price * item.Quantity;
            if (total == 0) { OnInsufficientFunds?.Invoke("Выберите животное!"); return; }
            if (_viewModel.Budget < total) { OnInsufficientFunds?.Invoke("Недостаточно монет!"); return; }

            foreach (var item in _buyAnimals)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    var animal = CreateAnimalFromName(item.ItemName);
                    if (animal != null) _viewModel.BuyAnimal(animal);
                }
            }

            OnPurchaseSuccess?.Invoke("Животные куплены!");
            foreach (var item in _buyAnimals) item.Quantity = 0;
            UpdateBuyAnimalTotal();
            Close();
        }

        private void UpdateBuyResourceTotal()
        {
            int total = 0;
            foreach (var item in _buyResources) total += item.Price * item.Quantity;
            BuyResourceTotalText.Text = $"{total} монет";
        }
        private void BuyResourceConfirm_Click(object sender, RoutedEventArgs e)
        {
            int total = 0;
            foreach (var item in _buyResources) total += item.Price * item.Quantity;
            if (total == 0) { OnInsufficientFunds?.Invoke("Выберите ресурс!"); return; }
            if (_viewModel.Budget < total) { OnInsufficientFunds?.Invoke("Недостаточно монет!"); return; }

            _viewModel.Budget -= total;

            foreach (var item in _buyResources)
            {
                if (item.Quantity > 0 && _viewModel.Inventory.ContainsKey(item.ItemName))
                    _viewModel.Inventory[item.ItemName] += item.Quantity;
            }

            OnPurchaseSuccess?.Invoke("Ресурсы куплены!");
            foreach (var item in _buyResources) item.Quantity = 0;
            UpdateBuyResourceTotal();
            Close();
        }

        // ===== ПРОДАЖА ЖИВОТНЫХ =====
        private void SellAnimalPlus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item })
            {
                if (item.Quantity < item.OwnCount) item.Quantity++;
                UpdateSellAnimalTotal();
            }
        }
        private void SellAnimalMinus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item })
            {
                if (item.Quantity > 0) item.Quantity--;
                UpdateSellAnimalTotal();
            }
        }

        // ===== ПРОДАЖА СЫРЬЯ =====
        private void SellResourcePlus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item })
            {
                if (item.Quantity < item.OwnCount) item.Quantity++;
                UpdateSellResourceTotal();
            }
        }
        private void SellResourceMinus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: StoreItem item })
            {
                if (item.Quantity > 0) item.Quantity--;
                UpdateSellResourceTotal();
            }
        }
        private void UpdateSellAnimalTotal()
        {
            int total = 0;
            foreach (var item in _sellAnimals) total += item.SellPrice * item.Quantity;
            SellAnimalTotalText.Text = $"{total} монет";
        }
        private void SellAnimalConfirm_Click(object sender, RoutedEventArgs e)
        {
            int total = 0;
            var animalsSold = 0;
            foreach (var item in _sellAnimals)
            {
                if (item.Quantity > 0)
                {
                    var animal = _viewModel.Farm.FirstOrDefault(a => a.Name == item.ItemName);
                    if (animal != null)
                    {
                        _viewModel.Budget += item.SellPrice;
                        var visual = _viewModel.AnimalVisuals.FirstOrDefault(v => v.Animal == animal);
                        if (visual != null) { visual.Stop(); _viewModel.AnimalVisuals.Remove(visual); }
                        _viewModel.Farm.Remove(animal);
                        animalsSold++;
                    }
                    total += item.SellPrice * item.Quantity;
                }
            }

            if (animalsSold == 0) { OnInsufficientFunds?.Invoke("Выберите животное!"); return; }

            OnPurchaseSuccess?.Invoke("Животные проданы!");
            Close();
        }

        private void UpdateSellResourceTotal()
        {
            int total = 0;
            foreach (var item in _sellResources) total += item.SellPrice * item.Quantity;
            SellResourceTotalText.Text = $"{total} монет";
        }
        private void SellResourceConfirm_Click(object sender, RoutedEventArgs e)
        {
            int total = 0;
            foreach (var item in _sellResources)
            {
                if (item.Quantity > 0)
                {
                    _viewModel.Budget += item.SellPrice * item.Quantity;
                    if (_viewModel.Inventory.ContainsKey(item.ItemName))
                        _viewModel.Inventory[item.ItemName] -= item.Quantity;
                    total += item.SellPrice * item.Quantity;
                }
            }

            if (total == 0) { OnInsufficientFunds?.Invoke("Выберите ресурс!"); return; }

            OnPurchaseSuccess?.Invoke("Ресурсы проданы!");
            Close();
        }

        private IAnimal? CreateAnimalFromName(string itemName)
        {
            var random = new Random();
            return itemName switch
            {
                "Курица" => new Chicken($"Курица {Guid.NewGuid().ToString().Substring(0, 4)}"),
                "Овца" => new Ram($"Баран {Guid.NewGuid().ToString().Substring(0, 4)}"),
                "Корова" => random.Next(2) == 0
                    ? new Cow($"Корова {Guid.NewGuid().ToString().Substring(0, 4)}")
                    : new Bull($"Бык {Guid.NewGuid().ToString().Substring(0, 4)}") as IAnimal,
                "Баран" => new Ram($"Баран {Guid.NewGuid().ToString().Substring(0, 4)}"),
                _ => null
            };
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
    }
}
