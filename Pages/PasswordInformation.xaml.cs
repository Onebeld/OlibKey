using System;
using System.Windows;

namespace OlibPasswordManager.Pages
{
    /// <summary>
    /// Логика взаимодействия для PasswordInformation.xaml
    /// </summary>
    public partial class PasswordInformation
    {
        public PasswordInformation() => InitializeComponent();

        private void ChangedPassword(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new Uri("/Pages/ChangePassword.xaml", UriKind.Relative));
    }
}
