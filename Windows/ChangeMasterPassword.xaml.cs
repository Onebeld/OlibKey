using Newtonsoft.Json;
using OlibPasswordManager.Properties.Core;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace OlibPasswordManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChangeMasterPassword.xaml
    /// </summary>
    public partial class ChangeMasterPassword
    {
        public ChangeMasterPassword() => InitializeComponent();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var s = File.ReadAllText(Additional.GlobalSettings.AppGlobalString);
                User.UsersList = JsonConvert.DeserializeObject<List<User>>(Encryptor.DecryptString(Encryptor.DecryptString(Encryptor.DecryptString(Encryptor.DecryptString(Encryptor.DecryptString(s, TxtOldPassword.Password), TxtOldPassword.Password), TxtOldPassword.Password), TxtOldPassword.Password), TxtOldPassword.Password));

                s = JsonConvert.SerializeObject(User.UsersList);
                App.MainWindow.MasterPassword = TxtPassword.Password;

                File.WriteAllText(Additional.GlobalSettings.AppGlobalString, Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(s, App.MainWindow.MasterPassword), App.MainWindow.MasterPassword), App.MainWindow.MasterPassword), App.MainWindow.MasterPassword), App.MainWindow.MasterPassword));

                MessageBox.Show((string)Application.Current.Resources["Successfully"], (string)Application.Current.Resources["Message"], MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch
            {
                MessageBox.Show((string)Application.Current.Resources["MB3"], (string)Application.Current.Resources["Error"], MessageBoxButton.OK, MessageBoxImage.Error);
                App.MainWindow.MasterPassword = null;
            }
        }

        private void CollapsedPassword(object sender, RoutedEventArgs e)
        {
            if (CbHide.IsChecked != null && (bool)CbHide.IsChecked)
            {
                TxtPassword.Visibility = Visibility.Collapsed;
                TxtPasswordCollapsed.Text = TxtPassword.Password;
                TxtPasswordCollapsed.Visibility = Visibility.Visible;
            }
            else if (CbHide.IsChecked != null && !(bool)CbHide.IsChecked)
            {
                TxtPassword.Visibility = Visibility.Visible;
                TxtPasswordCollapsed.Visibility = Visibility.Collapsed;
                TxtPasswordCollapsed.Text = string.Empty;
            }
        }

        private void OldCollapsedPassword(object sender, RoutedEventArgs e)
        {
            if (CbOldHide.IsChecked != null && (bool)CbOldHide.IsChecked)
            {
                TxtOldPassword.Visibility = Visibility.Collapsed;
                TxtOldPasswordCollapsed.Text = TxtPassword.Password;
                TxtOldPasswordCollapsed.Visibility = Visibility.Visible;
            }
            else if (CbOldHide.IsChecked != null && !(bool)CbOldHide.IsChecked)
            {
                TxtOldPassword.Visibility = Visibility.Visible;
                TxtOldPasswordCollapsed.Visibility = Visibility.Collapsed;
                TxtOldPasswordCollapsed.Text = string.Empty;
            }
        }

        private void txtOldPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CbHide.IsChecked != null && (bool)CbHide.IsChecked) TxtOldPassword.Password = TxtOldPasswordCollapsed.Text;
        }

        private void txtPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CbHide.IsChecked != null && (bool)CbHide.IsChecked) TxtPassword.Password = TxtPasswordCollapsed.Text;
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e) => PbHard.Value = PasswordUtils.CheckPasswordStrength(TxtPassword.Password);
        private void pbHard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => ItemControls.ColorProgressBar(PbHard);
    }
}
