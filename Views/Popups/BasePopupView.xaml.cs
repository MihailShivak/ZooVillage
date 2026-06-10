using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ZooVillage.Views.Popups
{
    public partial class BasePopupView : UserControl
    {
        public event Action OnCloseRequested;

        public BasePopupView()
        {
            InitializeComponent();
        }

        public void SetTitle(string title)
        {
            PopupTitle.Text = title;
        }

        public void SetContent(UserControl content)
        {
            PopupContent.Content = content;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            OnCloseRequested?.Invoke();
        }

        private void Overlay_Click(object sender, MouseButtonEventArgs e)
        {
            if (ReferenceEquals(e.Source, sender))
            {
                OnCloseRequested?.Invoke();
            }
        }
    }
}
