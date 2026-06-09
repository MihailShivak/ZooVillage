using System;
using System.Windows;
using ZooVillage.Views;

namespace ZooVillage
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            MainContent.Content = new MainView(this);
        }

        public void ShowGame()
        {
            MainContent.Content = new GameView(this);
        }

        public void ExitGame()
        {
            Application.Current.Shutdown();
        }
    }
}