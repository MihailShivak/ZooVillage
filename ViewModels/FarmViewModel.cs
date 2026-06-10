using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using ZooVillage.Models.Animals.Birds;
using ZooVillage.Models.Animals.Mammals;
using ZooVillage.Models.Interfaces.Base;
using ZooVillage.Services;
using ZooVillage.Views.Models;

namespace ZooVillage.ViewModels
{
    public class FarmViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<IAnimal> Farm { get; } = new ObservableCollection<IAnimal>();
        public ObservableCollection<AnimalVisual> AnimalVisuals { get; } = new ObservableCollection<AnimalVisual>();
        public BreedingService BreedingService { get; } = new BreedingService();
        public AudioManager AudioManager { get; } = new AudioManager();

        // Инвентарь ресурсов
        public Dictionary<string, int> Inventory { get; } = new Dictionary<string, int>
        {
            { "Яйцо", 15 },
            { "Молоко", 8 },
            { "Шерсть", 5 },
            { "Зерно", 20 }
        };

        private DispatcherTimer _simulationTimer;
        private DispatcherTimer _breedingTimer;
        private DispatcherTimer _productionTimer;

        private double _budget;
        public double Budget
        {
            get => _budget;
            set
            {
                _budget = value;
                OnPropertyChanged();
            }
        }

        private string _log;
        public string Log
        {
            get => _log;
            set
            {
                _log = value;
                OnPropertyChanged();
            }
        }

        public FarmViewModel()
        {
            Budget = 200000;
            InitializeFarm();
            InitializeTimers();
            InitializeAudio();
        }

        private void InitializeAudio()
        {
            System.Diagnostics.Debug.WriteLine("🎵 Инициализация аудиосистемы...");

            if (AudioManager.MusicLoaded)
            {
                AudioManager.PlayBackgroundMusic();
                Log += "\n🎵 Фоновая музыка загружена и запущена";
                System.Diagnostics.Debug.WriteLine("✓ Музыка успешно запущена");
            }
            else
            {
                Log += "\n⚠ Не удалось загрузить музыку. Проверьте наличие файла Assets/Sounds/mz.mp3";
                System.Diagnostics.Debug.WriteLine("❌ Музыка не загружена");
            }
        }

        private void InitializeFarm()
        {
            var cow = new Cow("Буренка");
            var bull = new Bull("Бык");
            var ram = new Ram("Бараш");
            var chicken = new Chicken("Ряба");
            var rooster = new Rooster("Петя");

            Farm.Add(cow);
            Farm.Add(bull);
            Farm.Add(ram);
            Farm.Add(chicken);
            Farm.Add(rooster);

            // Создаём визуальные модели
            AnimalVisuals.Add(new AnimalVisual(cow));
            AnimalVisuals.Add(new AnimalVisual(bull));
            AnimalVisuals.Add(new AnimalVisual(ram));
            AnimalVisuals.Add(new AnimalVisual(chicken));
            AnimalVisuals.Add(new AnimalVisual(rooster));

            Log = " Ферма инициализирована. Запущена симуляция...\n";
        }

        private void InitializeTimers()
        {
            _simulationTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _simulationTimer.Tick += SimulationTimer_Tick;
            _simulationTimer.Start();

            var breedingInterval = TimeSpan.FromSeconds(30);

            _breedingTimer = new DispatcherTimer
            {
                Interval = breedingInterval
            };
            _breedingTimer.Tick += BreedingTimer_Tick;
            _breedingTimer.Start();

            // Таймер производства сырья каждые 3 секунды
            _productionTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            _productionTimer.Tick += ProductionTimer_Tick;
            _productionTimer.Start();
        }

        private void SimulationTimer_Tick(object sender, EventArgs e)
        {
            PassOneDay();
        }

        private void BreedingTimer_Tick(object sender, EventArgs e)
        {
            AttemptBreeding();
        }

        private void ProductionTimer_Tick(object sender, EventArgs e)
        {
            ProduceResources();
        }

        public void PassOneDay()
        {
            var animalsToProcess = Farm.ToList();

            foreach (var animal in animalsToProcess)
            {
                animal.AgeOneDay();

                if (animal is IJuvenile juvenile)
                {
                    var maturedAnimal = juvenile.TryMature();
                    if (maturedAnimal != null)
                    {
                        Farm.Remove(animal);
                        if (juvenile.Parent != null)
                        {
                            juvenile.Parent.Children.Remove(animal);
                        }
                        Farm.Add(maturedAnimal);
                        AddToLog($"🎂 {animal.Name} повзрослел и стал {maturedAnimal.GetType().Name}!");
                    }
                }
            }
        }

        public void AttemptBreeding()
        {
            var newborns = BreedingService.TryBreed(Farm);

            foreach (var newborn in newborns)
            {
                Farm.Add(newborn);
                AnimalVisuals.Add(new AnimalVisual(newborn));
                AddToLog($" Родился новый житель фермы: {newborn.Name}!");

                // Показываем сердечко у родителя (поищем по совпадению типа и пола)
                var parent = Farm.FirstOrDefault(a => a != newborn && a.GetType().Name == newborn.GetType().Name);
                if (parent != null)
                {
                    var parentVisual = AnimalVisuals.FirstOrDefault(v => v.Animal == parent);
                    parentVisual?.ShowBreeding();
                }
            }

            if (newborns.Count > 0)
            {
                OnPropertyChanged(nameof(Farm));
            }
        }

        private void AddToLog(string message)
        {
            Log += $"{DateTime.Now:HH:mm:ss} - {message}\n";
            if (Log.Length > 5000)
            {
                Log = Log.Substring(Log.Length - 3000);
            }
        }

        public void BuyAnimal(IAnimal animal)
        {
            double price = animal.CalculateCurrentPrice();
            if (Budget >= price)
            {
                Budget -= price;
                Farm.Add(animal);
                AnimalVisuals.Add(new AnimalVisual(animal));
                AddToLog($"💰 Куплено животное: {animal.Name} за {price:C}");
            }
            else
            {
                AddToLog($"❌ Недостаточно средств для покупки {animal.Name}");
            }
        }

        public void SellAnimal(IAnimal animal)
        {
            double price = animal.CalculateCurrentPrice();
            Budget += price;
            var visual = AnimalVisuals.FirstOrDefault(v => v.Animal == animal);
            if (visual != null)
            {
                visual.Stop();
                AnimalVisuals.Remove(visual);
            }
            Farm.Remove(animal);
            AddToLog($" Продано животное: {animal.Name} за {price:C}");
        }

        private void ProduceResources()
        {
            int milkProduced = 0;
            int eggsProduced = 0;
            int woolProduced = 0;

            foreach (var animal in Farm)
            {
                // Коровы дают молоко
                if (animal is Cow cow && cow.DailyMilkYield > 0)
                {
                    milkProduced += (int)cow.DailyMilkYield;
                }
                // Куры дают яйца
                else if (animal is Chicken chicken && chicken.EggsPerWeek > 0)
                {
                    eggsProduced += chicken.EggsPerWeek / 7; // За 3 секунды (примерно 1 яйцо за несколько секунд)
                }
                // Бараны дают шерсть
                else if (animal is Ram ram && ram.WoolPerYear > 0)
                {
                    woolProduced += (int)(ram.WoolPerYear / 120000); // За 3 секунды (примерно)
                    if (woolProduced == 0) woolProduced = 1; // Минимум 1 единица
                }
            }

            // Добавляем в инвентарь
            if (milkProduced > 0)
            {
                Inventory["Молоко"] += milkProduced;
                if (milkProduced > 0) AddToLog($"🥛 Произведено молока: +{milkProduced}л");
            }

            if (eggsProduced > 0)
            {
                Inventory["Яйцо"] += eggsProduced;
                if (eggsProduced > 0) AddToLog($"🥚 Снесено яиц: +{eggsProduced}шт");
            }

            if (woolProduced > 0)
            {
                Inventory["Шерсть"] += woolProduced;
                if (woolProduced > 0) AddToLog($"🧶 Произведено шерсти: +{woolProduced}кг");
            }

            OnPropertyChanged(nameof(Inventory));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}