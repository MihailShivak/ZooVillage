using System.Windows;

namespace ZooVillage.Views.Windows
{
    public abstract class BaseWindow : Window
    {
        public abstract string WindowTitle { get; }

        protected BaseWindow()
        {
            Title = WindowTitle;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.NoResize;
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
        }
    }
}
