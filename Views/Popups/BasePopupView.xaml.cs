using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ZooVillage.Views.Popups
{
    public partial class BasePopupView : UserControl
    {
        // Событие, которое сообщит GameView, что попап нужно скрыть
        public event Action OnCloseRequested;

        public BasePopupView()
        {
            InitializeComponent();
        }

        // Метод для установки заголовка извне
        public void SetTitle(string title)
        {
            PopupTitle.Text = title;
        }

        // Метод для подгрузки конкретного содержимого (Композиция!)
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
            // Закрытие при клике на затемненный фон
            OnCloseRequested?.Invoke();
        }

        // Простейшая реализация перетаскивания окна
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Window.GetWindow(this)?.DragMove();
            }
        }
    }
}