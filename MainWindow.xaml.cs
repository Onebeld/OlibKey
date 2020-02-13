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
using System.ComponentModel;

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

        private string PageTitle;
        #endregion
        public MainWindow() => InitializeComponent();
        #region OpenWindow

        private void OpenAboutWindow(object sender, RoutedEventArgs e) => new About().ShowDialog();
        private void OpenSettingsWindow(object sender, RoutedEventArgs e) => new Windows.Settings().ShowDialog();

        private void OpenPasswordGeneratorWindow(object sender, RoutedEventArgs e) =>
            new PasswordGenerator().ShowDialog();

        private void OpenRequireMasterPassword() => new RequireMasterPassword().ShowDialog();

        private void OpenChangeMasterPassword(object sender, RoutedEventArgs e) =>
            new ChangeMasterPassword().ShowDialog();

        #endregion
        #region Timers
        private void TimerAutoSafe(object sender, EventArgs e) => Save(false);
        #endregion
        private void ClosedApplication(object sender, RoutedEventArgs e)
        {
            File.WriteAllText("settings.json", JsonConvert.SerializeObject(Additional.GlobalSettings));
            Save(false);

            Application.Current.Shutdown();
        }

        private void CreatePassword(object sender, RoutedEventArgs e)
        {
            _passwordPage = new CreatePassword();
            FrameWindow.NavigationService.Navigate(_passwordPage);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using var sw = new StreamWriter("Build.txt");
            sw.Write("1.3.0.250");

            User.UsersList = new List<User>();

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

            if (Additional.GlobalSettings.AppGlobalString != null)
            {
                UnlockMenuItem.IsEnabled = true;
                UnlockNotifyIcon.IsEnabled = true;
            }
            else
            {
                UnlockMenuItem.IsEnabled = false;
                UnlockNotifyIcon.IsEnabled = false;
            }

            timer.Tick += TimerAutoSafe;
            timer.Interval = new TimeSpan(0, 3, 0);
            timer.Start();

            CheckUpdate(false);
            if (Additional.GlobalSettings.AppGlobalString != null)
                if (File.Exists(Additional.GlobalSettings.AppGlobalString))
                    new RequireMasterPassword().ShowDialog();
        }

        private void OpenCreateData(object sender, RoutedEventArgs e)
        {
            CreateData data = new CreateData();
            if (!(bool)data.ShowDialog()) return;
            Additional.GlobalSettings.AppGlobalString = data.TxtPathSelection.Text;
            Global.MasterPassword = data.TxtPassword.Password;

            string json = JsonConvert.SerializeObject(User.UsersList);
            File.WriteAllText(Additional.GlobalSettings.AppGlobalString, Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(Encryptor.EncryptString(json, Global.MasterPassword), Global.MasterPassword), Global.MasterPassword), Global.MasterPassword), Global.MasterPassword));
        }

        private void SaveBase(object sender, RoutedEventArgs e) => Save(true);

        private void Save(bool b)
        {
            try
            {
                string json = JsonConvert.SerializeObject(User.UsersList);
                File.WriteAllText(Additional.GlobalSettings.AppGlobalString,
                    Encryptor.EncryptString(
                        Encryptor.EncryptString(
                            Encryptor.EncryptString(
                                Encryptor.EncryptString(Encryptor.EncryptString(json, Global.MasterPassword),
                                    Global.MasterPassword), Global.MasterPassword), Global.MasterPassword),
                        Global.MasterPassword));
            }
            catch
            {
                if (b)
                    MessageBox.Show((string)FindResource("MB1"),
                        (string)FindResource("Error"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenBase(object sender, RoutedEventArgs e) => DopOpenBase();

        private void DopOpenBase()
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "Olib-files (*.olib)|*.olib"
            };
            if (!(bool)fileDialog.ShowDialog()) return;
            Additional.GlobalSettings.AppGlobalString = fileDialog.FileName;
            OpenRequireMasterPassword();
        }

        private void PasswordList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PasswordList.SelectedItem == null) return;

            User.IndexUser = PasswordList.SelectedIndex;

            if (_passwordInformation is null)
            {
                _passwordInformation = new PasswordInformation();
                FrameWindow.NavigationService.Navigate(_passwordInformation);
            }
            if (PageTitle != "JGftyILgb458")
            {
                FrameWindow.NavigationService.Navigate(_passwordInformation);
            }

            _passwordInformation.PageControl.Load();

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            File.WriteAllText("settings.json", JsonConvert.SerializeObject(Additional.GlobalSettings));
            Save(false);

            if (Additional.GlobalSettings.CollapseOnClose)
            {
                Application.Current.MainWindow.Hide();
                e.Cancel = true;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _passwordPage = new CreatePassword();
            FrameWindow.NavigationService.Navigate(_passwordPage);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control) return;
            switch (e.Key)
            {
                case Key.N:
                    _passwordPage = new CreatePassword();
                    FrameWindow.NavigationService.Navigate(_passwordPage);
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

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e) => PasswordList.SelectedItem = User.UsersList.FirstOrDefault(x => x.Name == TxtSearch.Text);

        private void CheckUpdateButton(object sender, RoutedEventArgs e) => CheckUpdate(true);

        private async void CheckUpdate(bool b)
        {
            try
            {
                using var wb = new WebClient();
                wb.DownloadStringCompleted += (s, args) => _str = args.Result;
                await wb.DownloadStringTaskAsync(new Uri("https://raw.githubusercontent.com/MagnificentEagle/OlibPasswordManager/master/forRepository/version.txt"));
                var latest = float.Parse(_str.Replace(".", ""));
                var current = float.Parse(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
                if (!(latest > current) && b)
                {
                    MessageBox.Show((string)FindResource("MB8"),
                        (string)FindResource("Message"), MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }
                if (!(latest > current)) return;
                if (MessageBox.Show((string)FindResource("MB4"),
                        (string)FindResource("Message"), MessageBoxButton.YesNo,
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
                if (b)
                {
                    MessageBox.Show((string)FindResource("MB5"),
                        (string)FindResource("Error"), MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow == null) return;
            Application.Current.MainWindow.Visibility = Visibility.Visible;
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            Application.Current.MainWindow.Topmost = true;
            Application.Current.MainWindow.Topmost = false;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            BorderInformation.Visibility = Visibility.Collapsed;
            PasswordListNotifyIcon.SelectedIndex = -1;
        }

        private void PasswordListNotifyIcon_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PasswordListNotifyIcon.SelectedIndex == -1) return;
            User.IndexUser = PasswordListNotifyIcon.SelectedIndex;
            NotifyControl.Load();
            BorderInformation.Visibility = Visibility.Visible;
        }

        private void UnlockMenuItem_OnClick(object sender, RoutedEventArgs e) => OpenRequireMasterPassword();

        private void LockMenuItem_OnClick(object sender, RoutedEventArgs e) => LockPasswordBase();

        private void LockPasswordBase()
        {
            Save(true);

            FrameWindow.NavigationService.Navigate(new Uri("/Pages/StartScreen.xaml", UriKind.Relative));

            AddButton.IsEnabled = false;

            SaveMenuItem.IsEnabled = false;
            ChangeMenuItem.IsEnabled = false;
            NewLoginMenuItem.IsEnabled = false;
            UnlockMenuItem.IsEnabled = true;
            LockMenuItem.IsEnabled = false;

            LockNotifyIcon.IsEnabled = false;
            UnlockNotifyIcon.IsEnabled = true;

            Global.MasterPassword = null;

            PasswordListNotifyIcon.ItemsSource = null;
            PasswordList.ItemsSource = null;
            User.UsersList.Clear();
        }

        private void FrameWindow_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e) => PageTitle = ((Page)e.Content).Title;
    }
}