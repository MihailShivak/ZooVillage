using System.Windows;
using System.Windows.Controls;

namespace ZooVillage.Views
{
    public partial class MainView : UserControl
    {
        private readonly MainWindow _mainWindow;

        public MainView(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e) => _mainWindow.ShowGame();
        private void AboutButton_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Окно 'О проекте' (Popup)");
        private void ExitButton_Click(object sender, RoutedEventArgs e) => _mainWindow.ExitGame();
    }
}