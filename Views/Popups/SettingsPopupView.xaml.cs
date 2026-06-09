using System.Windows;
using System.Windows.Controls;

namespace ZooVillage.Views.Popups
{
    public partial class SettingsPopupView : UserControl
    {
        public event Action? OnResumeGame;
        public event Action? OnAbout;
        public event Action? OnExitToMenu;

        public SettingsPopupView()
        {
            InitializeComponent();
        }

        private void Resume_Click(object sender, RoutedEventArgs e) => OnResumeGame?.Invoke();
        private void About_Click(object sender, RoutedEventArgs e) => OnAbout?.Invoke();
        private void ExitToMenu_Click(object sender, RoutedEventArgs e) => OnExitToMenu?.Invoke();
    }
}
