using System.Windows;
using System.Windows.Controls;

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
            // Здесь будет логика открытия PopupWindow Storage
            MessageBox.Show("Открыть окно Амбара (Popup 800x600)");
        }

        private void StoreButton_Click(object sender, RoutedEventArgs e)
        {
            // Здесь будет логика открытия PopupWindow Store
            MessageBox.Show("Открыть окно Магазина (Popup 800x600)");
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            // Здесь будет логика открытия PopupWindow Settings
            MessageBox.Show("Открыть окно Настроек (Popup 800x600)");
        }
    }
}