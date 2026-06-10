using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ZooVillage.Views.Windows
{
    public enum NotificationType { Success, Error }

    public partial class NotificationWindow : Window
    {
        public NotificationWindow(string message, NotificationType type)
        {
            InitializeComponent();

            MessageText.Text = message;

            if (type == NotificationType.Success)
            {
                RootBorder.Background = new SolidColorBrush(Color.FromArgb(230, 39, 174, 96));
                IconImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Icons/verified.png"));
            }
            else
            {
                RootBorder.Background = new SolidColorBrush(Color.FromArgb(230, 192, 57, 43));
                IconImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Icons/6514954.png"));
            }

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            timer.Tick += (s, e) => { timer.Stop(); Close(); };
            timer.Start();
        }

        public static void Show(string message, NotificationType type)
        {
            var w = new NotificationWindow(message, type);
            w.Show();
        }
    }
}
