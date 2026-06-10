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

namespace ZooVillage.ViewModels
{
    public class FarmViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<IAnimal> Farm { get; } = new ObservableCollection<IAnimal>();
        public BreedingService BreedingService { get; } = new BreedingService();
        public AudioManager AudioManager { get; } = new AudioManager();

        private DispatcherTimer _simulationTimer;
        private DispatcherTimer _breedingTimer;

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
            Farm.Add(new Cow("Буренка"));
            Farm.Add(new Bull("Бык"));
            Farm.Add(new Ram("Бараш"));
            Farm.Add(new Chicken("Ряба"));
            Farm.Add(new Rooster("Петя"));

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
        }

        private void SimulationTimer_Tick(object sender, EventArgs e)
        {
            PassOneDay();
        }

        private void BreedingTimer_Tick(object sender, EventArgs e)
        {
            AttemptBreeding();
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
                AddToLog($" Родился новый житель фермы: {newborn.Name}!");
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
            Farm.Remove(animal);
            AddToLog($" Продано животное: {animal.Name} за {price:C}");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}