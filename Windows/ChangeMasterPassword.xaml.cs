using Newtonsoft.Json;
using OlibPasswordManager.Properties.Core;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OlibPasswordManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChangeMasterPassword.xaml
    /// </summary>
    public partial class ChangeMasterPassword : Window
    {
        public ChangeMasterPassword() => InitializeComponent();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var s = File.ReadAllText(App.Settings.AppGlobalString);
                User.UsersList = JsonConvert.DeserializeObject<List<User>>(Encryptor.DecryptString(Encryptor.DecryptString(Encryptor.DecryptString(Encryptor.DecryptString(Encryptor.DecryptString(s, txtOldPassword.Password), txtOldPassword.Password), txtOldPassword.Password), txtOldPassword.Password), txtOldPassword.Password));

                s = JsonConvert.SerializeObject(User.UsersList);
                Global.MasterPassword = txtPassword.Password;

                File.WriteAllText(App.Settings.AppGlobalString, Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(s, Global.MasterPassword), Global.MasterPassword), Global.MasterPassword), Global.MasterPassword), Global.MasterPassword));

                MessageBox.Show((string)Application.Current.Resources["Successfully"], (string)Application.Current.Resources["Message"], MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch
            {
                MessageBox.Show((string)Application.Current.Resources["MB3"], (string)Application.Current.Resources["Error"], MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CollapsedPassword(object sender, RoutedEventArgs e)
        {
            if (cbHide.IsChecked != null && (bool)cbHide.IsChecked)
            {
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordCollapsed.Text = txtPassword.Password;
                txtPasswordCollapsed.Visibility = Visibility.Visible;
            }
            else if (cbHide.IsChecked != null && !(bool)cbHide.IsChecked)
            {
                txtPassword.Visibility = Visibility.Visible;
                txtPasswordCollapsed.Visibility = Visibility.Collapsed;
                txtPasswordCollapsed.Text = null;
            }
        }

        private void OldCollapsedPassword(object sender, RoutedEventArgs e)
        {
            if (cbHide.IsChecked != null && (bool)cbHide.IsChecked)
            {
                txtOldPassword.Visibility = Visibility.Collapsed;
                txtOldPasswordCollapsed.Text = txtPassword.Password;
                txtOldPasswordCollapsed.Visibility = Visibility.Visible;
            }
            else if (cbHide.IsChecked != null && !(bool)cbHide.IsChecked)
            {
                txtOldPassword.Visibility = Visibility.Visible;
                txtOldPasswordCollapsed.Visibility = Visibility.Collapsed;
            }
        }

        private void txtOldPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((bool)cbHide.IsChecked) txtOldPassword.Password = txtPasswordCollapsed.Text;
        }

        private void txtPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((bool)cbHide.IsChecked) txtPassword.Password = txtPasswordCollapsed.Text;
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e) => pbHard.Value = PasswordUtils.CheckPasswordStrength(txtPassword.Password);
        private void pbHard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (pbHard.Value < 100)
                pbHard.Foreground = new SolidColorBrush(Color.FromRgb(196, 20, 3));
            else if (pbHard.Value < 200)
                pbHard.Foreground = new SolidColorBrush(Color.FromRgb(222, 222, 64));
            else
                pbHard.Foreground = new SolidColorBrush(Color.FromRgb(27, 199, 11));
        }
    }
}
