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
        #region CopyText
        private void CopyPassword(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(TxtPassword.Password);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(TxtNameAccount.Text);
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(TxtWebSite.Text);
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(TxtCardNumber.Text);
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(TxtSecurityCode.Password);
        }
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
            sw.Write("1.2.0.229");

            User.UsersList = new List<User>();

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

            if (Additional.GlobalSettings.AppGlobalString != null)
            {
                UnlockMenuItem.IsEnabled = false;
                UnlockNotifyIcon.IsEnabled = false;
            }

            timer.Tick += TimerAutoSafe;
            timer.Interval = new TimeSpan(0, 3, 0);
            timer.Start();

            CheckUpdate(false);
            if (Additional.GlobalSettings.AppGlobalString != null) new RequireMasterPassword().ShowDialog();
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
                    MessageBox.Show((string) Application.Current.Resources["MB1"],
                        (string) Application.Current.Resources["Error"], MessageBoxButton.OK, MessageBoxImage.Error);
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
            _passwordInformation = new PasswordInformation();

            User.IndexUser = PasswordList.SelectedIndex;

            FrameWindow.NavigationService.Navigate(_passwordInformation);
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
                    MessageBox.Show((string) Application.Current.Resources["MB8"],
                        (string) Application.Current.Resources["Message"], MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }
                if (!(latest > current)) return;
                if (MessageBox.Show((string)Application.Current.Resources["MB4"],
                        (string)Application.Current.Resources["Message"], MessageBoxButton.YesNo,
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
                    MessageBox.Show((string) Application.Current.Resources["MB5"],
                        (string) Application.Current.Resources["Error"], MessageBoxButton.OK,
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

            TxtName.Text = User.UsersList[User.IndexUser].Name;
            TxtNameAccount.Text = User.UsersList[User.IndexUser].PasswordName;
            TxtPassword.Password = User.UsersList[User.IndexUser].Password;
            TxtWebSite.Text = User.UsersList[User.IndexUser].WebSite;
            LabelCreateData.Content = User.UsersList[User.IndexUser].TimeCreate;
            LabelChangeData.Content = User.UsersList[User.IndexUser].TimeChanged;
            TxtNote.Text = User.UsersList[User.IndexUser].Note;

            TxtCardName.Text = User.UsersList[User.IndexUser].CardName;
            TxtCardNumber.Text = User.UsersList[User.IndexUser].PasswordName;
            TxtDate.Text = User.UsersList[User.IndexUser].DateCard;
            TxtSecurityCode.Password = User.UsersList[User.IndexUser].SecurityCode;

            txtPassportName.Text = User.UsersList[User.IndexUser].PasswordName;
            txtPassportNumber.Text = User.UsersList[User.IndexUser].PassportNumber;
            txtPassportPlaceOfIssue.Text = User.UsersList[User.IndexUser].PassportPlaceOfIssue;

            BrNote.Visibility = TxtNote.Text == "" ? Visibility.Collapsed : Visibility.Visible;
            BWebSite.Visibility = TxtWebSite.Text == "" ? Visibility.Collapsed : Visibility.Visible;

            TxtLabelChange.Visibility = User.UsersList[User.IndexUser].TimeChanged == null ? Visibility.Collapsed : Visibility.Visible;
            switch (User.UsersList[User.IndexUser].Type)
            {
                case 0:
                    BCardName.Visibility = Visibility.Collapsed;
                    BCardNumber.Visibility = Visibility.Collapsed;
                    BDate.Visibility = Visibility.Collapsed;
                    BSecurityCode.Visibility = Visibility.Collapsed;
                    bPassportName.Visibility = Visibility.Collapsed;
                    bPassportNumber.Visibility = Visibility.Collapsed;
                    bPassportPlaceOfIssue.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    BUsername.Visibility = Visibility.Collapsed;
                    BPassword.Visibility = Visibility.Collapsed;
                    BWebSite.Visibility = Visibility.Collapsed;
                    bPassportName.Visibility = Visibility.Collapsed;
                    bPassportNumber.Visibility = Visibility.Collapsed;
                    bPassportPlaceOfIssue.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    BUsername.Visibility = Visibility.Collapsed;
                    BPassword.Visibility = Visibility.Collapsed;
                    BWebSite.Visibility = Visibility.Collapsed;
                    BCardName.Visibility = Visibility.Collapsed;
                    BCardNumber.Visibility = Visibility.Collapsed;
                    BDate.Visibility = Visibility.Collapsed;
                    BSecurityCode.Visibility = Visibility.Collapsed;
                    break;
            }

            BorderInformation.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "http://" + TxtWebSite.Text,
                UseShellExecute = true
            };
            Process.Start(psi);
        }


        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox b = (PasswordBox)sender;
            PbHard.Value = PasswordUtils.CheckPasswordStrength(b.Password);
        }

        private void pbHard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => ItemControls.ColorProgressBar(PbHard);

        private void TxtSecurityCodeCollapsed_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (CbSecurityCodeHide.IsChecked != null && (bool)CbSecurityCodeHide.IsChecked) TxtSecurityCode.Password = TxtSecurityCodeCollapsed.Text;
        }

        private void CbSecurityCodeHide_OnChecked(object sender, RoutedEventArgs e)
        {
            if (CbSecurityCodeHide.IsChecked != null && (bool)CbSecurityCodeHide.IsChecked)
            {
                TxtSecurityCode.Visibility = Visibility.Collapsed;
                TxtSecurityCodeCollapsed.Text = TxtPassword.Password;
                TxtSecurityCodeCollapsed.Visibility = Visibility.Visible;
            }
            else if (CbSecurityCodeHide.IsChecked != null && !(bool)CbSecurityCodeHide.IsChecked)
            {
                TxtSecurityCode.Visibility = Visibility.Visible;
                TxtSecurityCodeCollapsed.Visibility = Visibility.Collapsed;
                TxtSecurityCodeCollapsed.Text = string.Empty;
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

        private void UnlockMenuItem_OnClick(object sender, RoutedEventArgs e) => OpenRequireMasterPassword();

        private void LockMenuItem_OnClick(object sender, RoutedEventArgs e) => LockPasswordBase();

        private void LockPasswordBase()
        {
            Save(true);

            FrameWindow.NavigationService.Navigate(new Uri("/Pages/StartScreen.xaml", UriKind.Relative));

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
    }
}
