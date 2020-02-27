using OlibKey.AccountStructures;
using OlibKey.Core;
using OlibKey.ModelViews;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;

namespace OlibKey.Views
{
    /// <summary>
    /// Логика взаимодействия для ChangedPasswordPage.xaml
    /// </summary>
    public partial class ChangedPasswordPage : Page
    {
        public AccountModel AccountModelChange;
        public Action ChangedAccountCallback { get; set; }
        public Action DeleteAccountCallback { get; set; }
        private void ChangedAccountCallbackFunc() => ChangedAccountCallback?.Invoke();
        private void DeleteAccountCallbackFunc() => DeleteAccountCallback?.Invoke();
        public ChangedPasswordPage(AccountModel accountModel)
        {
            InitializeComponent();

            AccountModelChange = accountModel;

            DataContext = AccountModelChange;

            switch (accountModel.TypeAccount)
            {
                case 0:
                    BankCartSection.Visibility = Visibility.Collapsed;
                    PasportSection.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    PasswordSection.Visibility = Visibility.Collapsed;
                    PasportSection.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    PasswordSection.Visibility = Visibility.Collapsed;
                    BankCartSection.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void ChangedAccountClick(object sender, RoutedEventArgs e)
        {
            if (AccountModelChange.WebSite != null || AccountModelChange.WebSite != "")
            {
                AccountModelChange.IconWebSite = "http://www.google.com/s2/favicons?domain=" + AccountModelChange.WebSite;
            }
            AccountModelChange.TimeChanged = DateTime.Now.ToString();
            ChangedAccountCallbackFunc();
            NavigationService.GoBack();
        }

        private void PbHard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => ItemControls.ColorProgressBar(pbHard);

        private void DeletePassword(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить элемент?", "Сообщение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DeleteAccountCallbackFunc();
                StartPage startPage = new StartPage();
                NavigationService.Navigate(startPage);
            }
        }

        private void CancelChangedPassword(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void txtPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            pbHard.Value = PasswordUtils.CheckPasswordStrength(txtPasswordCollapsed.Text);
            if (!txtPassword.IsSelectionActive)
                txtPassword.Password = txtPasswordCollapsed.Text;
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtPassword.IsSelectionActive)
                txtPasswordCollapsed.Text = txtPassword.Password;
        }

        private void txtSecutityCode_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtSecutityCode.IsSelectionActive)
                txtSecutityCodeCollapsed.Text = txtSecutityCode.Password;
        }

        private void txtSecutityCodeCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtSecutityCode.Password = txtSecutityCodeCollapsed.Text;
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
    }
}
