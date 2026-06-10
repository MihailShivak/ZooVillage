using System.Windows;
using System.Windows.Controls;
using ZooVillage.Views.Windows;

namespace ZooVillage.Views
{
    public partial class GameView : UserControl
    {
        private readonly MainWindow _mainWindow;

        public GameView(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void StorageButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Открыть окно Амбара");
        }

        private void StoreButton_Click(object sender, RoutedEventArgs e)
        {
            var storeWindow = new StoreWindow();
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
