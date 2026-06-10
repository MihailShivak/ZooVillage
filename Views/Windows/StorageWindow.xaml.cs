using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using ZooVillage.Models.Animals.Birds;
using ZooVillage.Models.Animals.Mammals;
using ZooVillage.ViewModels;

namespace ZooVillage.Views.Windows
{
    public partial class StorageWindow : BaseWindow
    {
        public override string WindowTitle => "Амбар";

        private readonly FarmViewModel _viewModel;

        public StorageWindow(FarmViewModel viewModel)
        {
            InitializeComponent();
            Title = WindowTitle;
            _viewModel = viewModel;
            InitializeResources(viewModel);
        }

        private void InitializeResources(FarmViewModel viewModel)
        {
            var resources = new ObservableCollection<StorageItem>();

            // Животные
            foreach (var animal in viewModel.Farm)
            {
                var imagePath = GetAnimalImagePath(animal.GetType().Name);
                resources.Add(new StorageItem
                {
                    Name = animal.Name,
                    Quantity = 1,
                    ImagePath = imagePath
                });
            }

            // Ресурсы из инвентаря
            var inventory = new[]
            {
                new StorageItem { Name = "Яйцо", Quantity = viewModel.Inventory["Яйцо"], ImagePath = "/Assets/Items/egg.png" },
                new StorageItem { Name = "Молоко", Quantity = viewModel.Inventory["Молоко"], ImagePath = "/Assets/Items/milk.png" },
                new StorageItem { Name = "Шерсть", Quantity = viewModel.Inventory["Шерсть"], ImagePath = "/Assets/Items/wool.png" },
                new StorageItem { Name = "Зерно", Quantity = viewModel.Inventory["Зерно"], ImagePath = "/Assets/Items/hay.png" }
            };

            foreach (var item in inventory)
                resources.Add(item);

            ResourceList.ItemsSource = resources;

            // Обновляем инвентарь каждые 3 секунды
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            timer.Tick += (_, _) => RefreshInventory(resources, viewModel);
            timer.Start();
        }

        private void RefreshInventory(ObservableCollection<StorageItem> resources, FarmViewModel viewModel)
        {
            // Находим ресурсы в коллекции и обновляем их количество
            var egg = resources.FirstOrDefault(r => r.Name == "Яйцо");
            var milk = resources.FirstOrDefault(r => r.Name == "Молоко");
            var wool = resources.FirstOrDefault(r => r.Name == "Шерсть");
            var grain = resources.FirstOrDefault(r => r.Name == "Зерно");

            if (egg != null) egg.Quantity = viewModel.Inventory["Яйцо"];
            if (milk != null) milk.Quantity = viewModel.Inventory["Молоко"];
            if (wool != null) wool.Quantity = viewModel.Inventory["Шерсть"];
            if (grain != null) grain.Quantity = viewModel.Inventory["Зерно"];
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

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
    }

    public class StorageItem
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }
}
