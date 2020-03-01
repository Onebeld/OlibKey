using OlibKey.AccountStructures;
using OlibKey.Core;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace OlibKey.Views
{
    /// <summary>
    /// Логика взаимодействия для PasswordInformationPage.xaml
    /// </summary>
    public partial class PasswordInformationPage : Page
    {
        private readonly AccountModel AccountModel;
        public Action DeletedAccount { get; set; }
        public Action ChangedAccount { get; set; }
        private ChangedPasswordPage ChangedPasswordPage;
        public PasswordInformationPage(AccountModel model)
        {
            InitializeComponent();

            AccountModel = model;

            DataContext = AccountModel;

            switch (model.TypeAccount)
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
            if (model.TimeChanged == null)
                spDateChanged.Visibility = Visibility.Collapsed;
        }

        private void PbHard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => ItemControls.ColorProgressBar(pbHard);

        private void txtPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            pbHard.Value = PasswordUtils.CheckPasswordStrength(txtPasswordCollapsed.Text);
            txtPassword.Password = txtPasswordCollapsed.Text;
        }

        private void txtSecutityCodeCollapsed_TextChanged(object sender, TextChangedEventArgs e) => txtSecutityCode.Password = txtSecutityCodeCollapsed.Text;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "http://" + AccountModel.WebSite,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void CopyTexts(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.Clear();
                switch (int.Parse(((Button)e.Source).Uid))
                {
                    case 1: Clipboard.SetText(AccountModel.Password); break;
                    case 2: Clipboard.SetText(AccountModel.WebSite); break;
                    case 3: Clipboard.SetText(AccountModel.Username); break;
                    case 4: Clipboard.SetText(AccountModel.SecurityCode); break;
                    case 5: Clipboard.SetText(AccountModel.Username); break;
                    case 6: Clipboard.SetText(AccountModel.PassportNumber); break;
                    case 7: Clipboard.SetText(AccountModel.PassportPlaceOfIssue); break;
                }
            }
            catch { }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChangedPasswordPage = new ChangedPasswordPage(AccountModel)
            {
                DeleteAccountCallback = DeletedAccount,
                ChangedAccountCallback = ChangedAccount,
            };
            NavigationService?.Navigate(ChangedPasswordPage);
        }
    }
}
