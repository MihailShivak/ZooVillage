using System.Windows;

namespace ZooVillage.Views.Windows
{
    public partial class SettingsWindow : BaseWindow
    {
        public override string WindowTitle => "Настройки";

        public event Action? OnExitToMenu;

        public SettingsWindow()
        {
            InitializeComponent();
            Title = WindowTitle;
        }

        private void Resume_Click(object sender, RoutedEventArgs e) => Close();

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ZooVillage — игра про ферму с животными\n\nРазработано для курса ООП",
                            "О проекте", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExitToMenu_Click(object sender, RoutedEventArgs e)
        {
            OnExitToMenu?.Invoke();
            Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}
