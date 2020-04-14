using OlibKey.AccountStructures;
using OlibKey.Core;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OlibKey.Views
{
    /// <summary>
    /// Логика взаимодействия для CreatePasswordPage.xaml
    /// </summary>
    public partial class CreatePasswordPage : Page
    {
        public Account AccountModel = new Account();
        public Action AddAccountCallback { get; set; }
        private void AddAccountCallbackFunc() => AddAccountCallback?.Invoke();
        public CreatePasswordPage()
        {
            InitializeComponent();
            DataContext = AccountModel;
            
        }

        private void AddAccountClick(object sender, RoutedEventArgs e)
        {
            if (AccountModel.WebSite != null)
                AccountModel.IconWebSite = "http://www.google.com/s2/favicons?domain=" + AccountModel.WebSite;
            AccountModel.TimeCreate = DateTime.Now.ToString(System.Threading.Thread.CurrentThread.CurrentUICulture);
            AddAccountCallbackFunc();
            App.MainWindow.Notification((string)FindResource("Not1"));
        }

        private void PbHard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => ItemControls.ColorProgressBar(pbHard);

        private void ExitAddPassword(object sender, RoutedEventArgs e)
        {
            StartPage startPage = new StartPage();
            NavigationService?.Navigate(startPage);
        }

        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbType.SelectedIndex)
            {
                case 0:
                    PasswordSection.Visibility = Visibility.Visible;
                    BankCartSection.Visibility = Visibility.Collapsed;
                    PasportSection.Visibility = Visibility.Collapsed;
                    ReminderSection.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    PasswordSection.Visibility = Visibility.Collapsed;
                    BankCartSection.Visibility = Visibility.Visible;
                    PasportSection.Visibility = Visibility.Collapsed;
                    ReminderSection.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    PasswordSection.Visibility = Visibility.Collapsed;
                    BankCartSection.Visibility = Visibility.Collapsed;
                    PasportSection.Visibility = Visibility.Visible;
                    ReminderSection.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    PasswordSection.Visibility = Visibility.Collapsed;
                    BankCartSection.Visibility = Visibility.Collapsed;
                    PasportSection.Visibility = Visibility.Collapsed;
                    ReminderSection.Visibility = Visibility.Visible;
                    cbIsStartRemember.IsChecked = true;
                    tbStartTime.Text = DateTime.Now.ToString(System.Threading.Thread.CurrentThread.CurrentUICulture);
                    break;
            }
        }

        private void txtPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            pbHard.Value = PasswordUtils.CheckPasswordStrength(txtPasswordCollapsed.Text);
            if (txtPasswordCollapsed.IsSelectionActive)
                txtPassword.Password = txtPasswordCollapsed.Text;
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtPassword.IsSelectionActive)
                txtPasswordCollapsed.Text = txtPassword.Password;
        }
        private void txtSecutityCodeCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSecutityCodeCollapsed.IsSelectionActive)
                txtSecutityCode.Password = txtSecutityCodeCollapsed.Text;
        }

        private void txtSecutityCode_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtSecutityCode.IsSelectionActive)
                txtSecutityCodeCollapsed.Text = txtSecutityCode.Password;
        }


        private void cbHide_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)cbHide.IsChecked)
            {
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordCollapsed.Visibility = Visibility.Visible;
            }
            else
            {
                txtPassword.Visibility = Visibility.Visible;
                txtPasswordCollapsed.Visibility = Visibility.Collapsed;
            }
        }
        private void cbHideSecurityCode_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)cbHideSecurityCode.IsChecked)
            {
                txtSecutityCode.Visibility = Visibility.Collapsed;
                txtSecutityCodeCollapsed.Visibility = Visibility.Visible;
            }
            else
            {
                txtSecutityCode.Visibility = Visibility.Visible;
                txtSecutityCodeCollapsed.Visibility = Visibility.Collapsed;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            PasswordGeneratorWindow generatorWindow = new PasswordGeneratorWindow
            {
                SaveButton = {Visibility = Visibility.Visible}
            };
            if ((bool)generatorWindow.ShowDialog())
            {
                txtPassword.Password = generatorWindow.TxtPassword.Text;
                txtPasswordCollapsed.Text = generatorWindow.TxtPassword.Text;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) => tbStartTime.Text = DateTime.Now.ToString(System.Threading.Thread.CurrentThread.CurrentUICulture);
    }
}
