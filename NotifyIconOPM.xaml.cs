using System;
using System.Windows;

namespace OlibPasswordManager
{
    /// <summary>
    /// Логика взаимодействия для NotifyIconOPM.xaml
    /// </summary>
    public partial class NotifyIconOPM : Window
    {
        public NotifyIconOPM()
        {
            InitializeComponent();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            App.MainWindow.Visibility = Visibility.Visible;
        }

        private void MenuItem_OnClick1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
