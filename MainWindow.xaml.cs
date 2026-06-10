using System;
using System.Windows;
using ZooVillage.Views;
using ZooVillage.ViewModels;

namespace ZooVillage
{
    public partial class MainWindow : Window
    {
        private FarmViewModel _farmViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _farmViewModel = new FarmViewModel();
            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            MainContent.Content = new MainView(this);
        }

        public void ShowGame()
        {
            MainContent.Content = new GameView(this, _farmViewModel);
        }

        public void ExitGame()
        {
            Application.Current.Shutdown();
        }
    }
}