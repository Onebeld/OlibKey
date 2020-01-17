using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OlibPasswordManager.Properties.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OlibPasswordManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для RequireMasterPassword.xaml
    /// </summary>
    public partial class RequireMasterPassword : Window
    {
        public RequireMasterPassword() => InitializeComponent();

        private void Button_Click(object sender, RoutedEventArgs e) => Require();
        private void PressEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Require();
        }

        private void Require()
        {
            try
            {
                var s = File.ReadAllText(App.Settings.AppGlobalString);
                User.UsersList = JsonConvert.DeserializeObject<List<User>>(Encryptor.DecryptString(Encryptor.DecryptString(Encryptor.DecryptString(Encryptor.DecryptString(Encryptor.DecryptString(s, txtPassword.Password), txtPassword.Password), txtPassword.Password), txtPassword.Password), txtPassword.Password));
                App.MainWindow.PasswordList.ItemsSource = null;
                App.MainWindow.PasswordList.ItemsSource = User.UsersList;

                Global.MasterPassword = txtPassword.Password;

                App.Settings.AppGlobalString = App.Settings.AppGlobalString;

                s = null;

                Close();
            }
            catch
            {
                MessageBox.Show((string) Application.Current.Resources["MB3"],
                    (string) Application.Current.Resources["Error"], MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
