using System.Windows;
using System.Windows.Controls;
using ZooVillage.ViewModels;
using ZooVillage.Views.Windows;

namespace ZooVillage.Views
{
    public partial class GameView : UserControl
    {
        private readonly MainWindow _mainWindow;
        private readonly FarmViewModel _viewModel;

        public GameView(MainWindow mainWindow, FarmViewModel viewModel)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void StorageButton_Click(object sender, RoutedEventArgs e)
        {
            var storageWindow = new StorageWindow(_viewModel);
            storageWindow.ShowDialog();
        }

        private void StoreButton_Click(object sender, RoutedEventArgs e)
        {
            var storeWindow = new StoreWindow(_viewModel);
            storeWindow.OnPurchaseSuccess += msg => NotificationWindow.Show(msg, NotificationType.Success);
            storeWindow.OnInsufficientFunds += msg => NotificationWindow.Show(msg, NotificationType.Error);
            storeWindow.ShowDialog();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.OnExitToMenu += () => _mainWindow.ShowMainMenu();
            settingsWindow.ShowDialog();
        }
    }
}
