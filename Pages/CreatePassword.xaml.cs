using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OlibPasswordManager.Windows;

namespace OlibPasswordManager.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreatePassword.xaml
    /// </summary>
    public partial class CreatePassword : Page
    {
        public CreatePassword()
        {
            InitializeComponent();
        }

        private void OpenPasswordGeneration(object sender, RoutedEventArgs e)
        {
            PasswordGenerator generator = new PasswordGenerator();
            generator.saveButton.Visibility = Visibility.Visible;
            if ((bool)generator.ShowDialog()) txtPassword.Password = generator.txtPassword.Text;
        }

        private void CollapsedPassword(object sender, RoutedEventArgs e)
        {
            if ((bool)cbHide.IsChecked)
            {
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordCollapsed.Text = txtPassword.Password;
                txtPasswordCollapsed.Visibility = Visibility.Visible;
            }
            else if (!(bool)cbHide.IsChecked)
            {
                txtPassword.Visibility = Visibility.Visible;
                txtPasswordCollapsed.Visibility = Visibility.Collapsed;
                txtPasswordCollapsed.Text = null;
            }
        }

        private void txtPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPassword.Password = txtPasswordCollapsed.Text;
        }

        private void CloseCreatePassword(object sender, RoutedEventArgs e)
        {
            txtName.Text = null;
            txtNameAccount.Text = null;
            txtPassword.Password = null;
            txtPasswordCollapsed.Text = null;
            NavigationService.Navigate(new Uri("/Pages/StartScreen.xaml", UriKind.Relative));
        }
    }
}
