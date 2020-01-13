using Microsoft.Win32;
using Newtonsoft.Json;
using OlibPasswordManager.Pages;
using OlibPasswordManager.Properties.Core;
using OlibPasswordManager.Windows;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OlibPasswordManager
{
    public static class Global
    {
        public static string MasterPassword { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Pages
        private CreatePassword PasswordPage;
        private PasswordInformation PasswordInformation;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        #region OpenWindow
        private void OpenAboutWindow(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }

        private void OpenSettingsWindow(object sender, RoutedEventArgs e)
        {
            new Windows.Settings().ShowDialog();
        }

        private void OpenPasswordGeneratorWindow(object sender, RoutedEventArgs e)
        {
            new PasswordGenerator().ShowDialog();
        }

        private void OpenRequireMasterPassword()
        {
            new RequireMasterPassword().ShowDialog();
        }

        private void OpenChangeMasterPassword(object sender, RoutedEventArgs e)
        {
            new ChangeMasterPassword().ShowDialog();
        }

        #endregion

        private void ClosedApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CreatePassword(object sender, RoutedEventArgs e)
        {
            PasswordPage = new CreatePassword();
            frame.NavigationService.Navigate(PasswordPage);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using StreamWriter sw = new StreamWriter("Build.txt");
            sw.Write("1.1.0.116");

            App.Settings = new Properties.Core.Settings();

            User.UsersList = new List<User>();
            PasswordList.ItemsSource = User.UsersList;

            if (App.Settings.AppGlobalString != null)
            {
                new RequireMasterPassword().ShowDialog();
            }
        }

        private void OpenCreateData(object sender, RoutedEventArgs e)
        {
            CreateData data = new CreateData();
            if ((bool)data.ShowDialog())
            {
                App.Settings.AppGlobalString = data.txtPathSelection.Text;
                Global.MasterPassword = data.txtPassword.Password;

                string json = JsonConvert.SerializeObject(User.UsersList);
                File.WriteAllText(App.Settings.AppGlobalString, Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(json, Global.MasterPassword), Global.MasterPassword), Global.MasterPassword), Global.MasterPassword), Global.MasterPassword));
            }
        }

        private void SaveBase(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            try
            {
                string json = JsonConvert.SerializeObject(User.UsersList);
                File.WriteAllText(App.Settings.AppGlobalString, Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(json, Global.MasterPassword), Global.MasterPassword), Global.MasterPassword), Global.MasterPassword), Global.MasterPassword));
            }
            catch
            {
                MessageBox.Show((string)Application.Current.Resources["MB1"], (string)Application.Current.Resources["Error"], MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenBase(object sender, RoutedEventArgs e)
        {
            DopOpenBase();
        }

        private void DopOpenBase()
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "Olib-files (*.olib)|*.olib"
            };
            if ((bool)fileDialog.ShowDialog())
            {
                App.Settings.AppGlobalString = fileDialog.FileName;
                OpenRequireMasterPassword();
            }
        }

        private void PasswordList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PasswordInformation != null)
            {
                PasswordInformation = null;
            }

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
            try
            {
                Save();
            }
            catch { }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            PasswordPage = new CreatePassword();
            frame.NavigationService.Navigate(PasswordPage);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.N:
                        PasswordPage = new CreatePassword();
                        frame.NavigationService.Navigate(PasswordPage);
                        break;
                    case Key.G:
                        new PasswordGenerator().ShowDialog();
                        break;
                    case Key.O:
                        DopOpenBase();
                        break;
                }
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            PasswordList.SelectedItem = User.UsersList.FirstOrDefault(x => x.Name == txtSearch.Text);
        }
    }
}
