using Microsoft.Win32;
using Newtonsoft.Json;
using OlibPasswordManager.Pages;
using OlibPasswordManager.Properties.Core;
using OlibPasswordManager.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
    public partial class MainWindow
    {
        private string _str;
        #region Pages
        private CreatePassword _passwordPage;
        private PasswordInformation _passwordInformation;
        #endregion

        public MainWindow() => InitializeComponent();

        #region OpenWindow
        private void OpenAboutWindow(object sender, RoutedEventArgs e) => new About().ShowDialog();

        private void OpenSettingsWindow(object sender, RoutedEventArgs e) => new Windows.Settings().ShowDialog();

        private void OpenPasswordGeneratorWindow(object sender, RoutedEventArgs e) => new PasswordGenerator().ShowDialog();

        private static void OpenRequireMasterPassword() => new RequireMasterPassword().ShowDialog();

        private void OpenChangeMasterPassword(object sender, RoutedEventArgs e) => new ChangeMasterPassword().ShowDialog();

        #endregion

        private void ClosedApplication(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void CreatePassword(object sender, RoutedEventArgs e)
        {
            _passwordPage = new CreatePassword();
            frame.NavigationService.Navigate(_passwordPage);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using var sw = new StreamWriter("Build.txt");
            sw.Write("1.1.0.127");

            App.Settings = new Properties.Core.Settings();

            User.UsersList = new List<User>();
            PasswordList.ItemsSource = User.UsersList;

            if (App.Settings.AppGlobalString != null) new RequireMasterPassword().ShowDialog();
        }

        private void OpenCreateData(object sender, RoutedEventArgs e)
        {
            CreateData data = new CreateData();
            var b = data.ShowDialog();
            if (b != null && (bool) b) return;
            App.Settings.AppGlobalString = data.txtPathSelection.Text;
            Global.MasterPassword = data.txtPassword.Password;

            string json = JsonConvert.SerializeObject(User.UsersList);
            File.WriteAllText(App.Settings.AppGlobalString, Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(json, Global.MasterPassword), Global.MasterPassword), Global.MasterPassword), Global.MasterPassword), Global.MasterPassword));
        }

        private void SaveBase(object sender, RoutedEventArgs e) => Save(true);

        private void Save(bool b)
        {
            try
            {
                string json = JsonConvert.SerializeObject(User.UsersList);
                File.WriteAllText(App.Settings.AppGlobalString, Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(json, Global.MasterPassword), Global.MasterPassword), Global.MasterPassword), Global.MasterPassword), Global.MasterPassword));
            }
            catch
            {
                if (b)
                    MessageBox.Show((string) Application.Current.Resources["MB1"],
                        (string) Application.Current.Resources["Error"], MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenBase(object sender, RoutedEventArgs e)
        {
            DopOpenBase();
        }

        private void DopOpenBase()
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "Olib-files (*.olib)|*.olib"
            };
            var b = fileDialog.ShowDialog();
            if (b != null && (bool) b) return;
            App.Settings.AppGlobalString = fileDialog.FileName;
            OpenRequireMasterPassword();
        }

        private void PasswordList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_passwordInformation != null) _passwordInformation = null;

            if (PasswordList.SelectedItem == null) return;
            _passwordInformation = new PasswordInformation();

            User.IndexUser = PasswordList.SelectedIndex;

            frame.NavigationService.Navigate(_passwordInformation);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            File.WriteAllText("settings.json", JsonConvert.SerializeObject(App.Settings));
            try
            {
                Save(false);
            }
            catch
            {
                // ignored
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _passwordPage = new CreatePassword();
            frame.NavigationService.Navigate(_passwordPage);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control) return;
            switch (e.Key)
            {
                case Key.N:
                    _passwordPage = new CreatePassword();
                    frame.NavigationService.Navigate(_passwordPage);
                    break;
                case Key.G:
                    new PasswordGenerator().ShowDialog();
                    break;
                case Key.O:
                    DopOpenBase();
                    break;
                case Key.S:
                    Save(true);
                    break;
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e) => PasswordList.SelectedItem = User.UsersList.FirstOrDefault(x => x.Name == txtSearch.Text);

        private async void CheckUpdate(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var wb = new WebClient())
                {
                    wb.DownloadStringCompleted += (s, args) => _str = args.Result;
                    await wb.DownloadStringTaskAsync(new Uri($"https://raw.githubusercontent.com/BigBoss500/Olib/master/versions/version.xml"));
                }
                var latest = float.Parse(_str.Replace(".", ""));
                var current = float.Parse(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
                if (!(latest > current)) return;
                if (MessageBox.Show((string) Application.Current.Resources["MB4"],
                        (string) Application.Current.Resources["Message"], MessageBoxButton.YesNo,
                        MessageBoxImage.Information) != MessageBoxResult.Yes) return;
                var psi = new ProcessStartInfo
                {
                    FileName = "https://github.com/MagnificentEagle/OlibPasswordManager/releases",
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch
            {
                // ignored
            }
        }
    }
}
