using Microsoft.Win32;
using Newtonsoft.Json;
using OlibPasswordManager.Pages;
using OlibPasswordManager.Properties.Core;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace OlibPasswordManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string dir;
        string masterPassword;

        #region Pages
        private CreatePassword PasswordPage;
        private PasswordInformation PasswordInformation;
        #endregion

        public MainWindow() => InitializeComponent();

        #region OpenWindow
        private void OpenAboutWindow(object sender, RoutedEventArgs e) => new Windows.About().ShowDialog();
        private void OpenSettingsWindow(object sender, RoutedEventArgs e) => new Windows.Settings().ShowDialog();
        private void OpenPasswordGeneratorWindow(object sender, RoutedEventArgs e) => new Windows.PasswordGenerator().ShowDialog();
        #endregion

        private void ClosedApplication(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void CreatePassword(object sender, RoutedEventArgs e)
        {
            PasswordPage = new CreatePassword();
            frame.NavigationService.Navigate(PasswordPage);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using StreamWriter sw = new StreamWriter("Build.txt");
            sw.Write("1.0.0.50");

            App.Settings = new Settings();

            if (File.Exists("settings.json")) App.Settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("settings.json"));

            User.UsersList = new List<User>();
            User.UsersList.Add(new User
            {
                Name = "ВКонтакте",
                Note = "",
                Password = "1532142432",
                PasswordName = "dmitry",
                WebSite = "vk.com"
            });
            PasswordList.ItemsSource = User.UsersList;
        }

        private void OpenCreateData(object sender, RoutedEventArgs e)
        {
            Windows.CreateData data = new Windows.CreateData();
            if ((bool)data.ShowDialog())
            {
                dir = data.txtPathSelection.Text + "\\" + data.txtName.Text + ".aes";
                masterPassword = data.txtPassword.Password;
            }
        }

        private void SaveBase(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(dir))
            {
                Encryptor.FileEncrypt(dir, masterPassword, FileMode.Create);
            }
            else
            {
                Encryptor.FileEncrypt(dir, masterPassword, FileMode.Open);
            }
        }

        private void OpenBase(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if ((bool)fileDialog.ShowDialog())
            {

            }
        }

        private void PasswordList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PasswordInformation != null) PasswordInformation = null;

            if (PasswordList.SelectedItem != null)
            {
                PasswordInformation = new PasswordInformation();

                User.IndexUser = PasswordList.SelectedIndex;

                frame.NavigationService.Navigate(PasswordInformation);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            File.WriteAllText("settings.json", JsonConvert.SerializeObject(App.Settings));
        }
    }
}
