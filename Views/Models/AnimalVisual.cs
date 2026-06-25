using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using ZooVillage.Models.Interfaces.Base;

namespace ZooVillage.Views.Models
{
    public class AnimalVisual : INotifyPropertyChanged
    {
        private double _x;
        private double _y;
        private bool _isBreeding;
        private DispatcherTimer _moveTimer;
        private DispatcherTimer _breedingTimer;
        private readonly Random _random = new Random();
        private readonly IAnimal _animal;

        public IAnimal Animal => _animal;

        public double X
        {
            get => _x;
            set { _x = value; OnPropertyChanged(); }
        }

        public double Y
        {
            get => _y;
            set { _y = value; OnPropertyChanged(); }
        }

        public bool IsBreeding
        {
            get => _isBreeding;
            set { _isBreeding = value; OnPropertyChanged(); }
        }

        public AnimalVisual(IAnimal animal, double canvasWidth = 600, double canvasHeight = 600)
        {
            _animal = animal;
            X = _random.NextDouble() * (canvasWidth - 80);
            Y = _random.NextDouble() * (canvasHeight - 80);

            _moveTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            _moveTimer.Tick += (_, _) => MoveRandomly(canvasWidth, canvasHeight);
            _moveTimer.Start();
        }

        private void MoveRandomly(double maxX, double maxY)
        {
            // Случайное направление: -10, 0, +10
            int dirX = (_random.Next(3) - 1) * 10;
            int dirY = (_random.Next(3) - 1) * 10;

            X = Math.Max(0, Math.Min(maxX - 80, X + dirX));
            Y = Math.Max(0, Math.Min(maxY - 80, Y + dirY));
        }

        public void ShowBreeding()
        {
            IsBreeding = true;
            _breedingTimer?.Stop();
            _breedingTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            _breedingTimer.Tick += (_, _) =>
            {
                IsBreeding = false;
                _breedingTimer.Stop();
            };
            _breedingTimer.Start();
        }

        public void Stop()
        {
            _moveTimer?.Stop();
            _breedingTimer?.Stop();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
