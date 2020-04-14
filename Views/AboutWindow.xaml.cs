using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace OlibKey.Views
{
    /// <summary>
    /// Логика взаимодействия для AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow() => InitializeComponent();

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) => Version.Content =
            " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        private void Button_Click(object sender, RoutedEventArgs e) => Close();

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo { FileName = "https://vk.com/olibkey", UseShellExecute = true };
            Process.Start(psi);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://github.com/MagnificentEagle/OlibKey",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://www.facebook.com/olibkey/",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void Timeline_OnCompleted(object? sender, EventArgs e) => Close();
    }
}
