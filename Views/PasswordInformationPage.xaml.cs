using OlibKey.AccountStructures;
using OlibKey.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OlibKey.Views
{
    /// <summary>
    /// Логика взаимодействия для PasswordInformationPage.xaml
    /// </summary>
    public partial class PasswordInformationPage : Page
    {
        private readonly Account _accountModel;
        public Action DeletedAccount { get; set; }
        public Action ChangedAccount { get; set; }
        public ChangedPasswordPage ChangedPasswordPage;
        public PasswordInformationPage(Account model)
        {
            InitializeComponent();

            _accountModel = model;

            DataContext = _accountModel;

            txtPassword.Password = model.Password;

            switch (model.TypeAccount)
            {
                case 0:
                    BankCartSection.Visibility = Visibility.Collapsed;
                    PasportSection.Visibility = Visibility.Collapsed;
                    ReminderSection.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    PasswordSection.Visibility = Visibility.Collapsed;
                    PasportSection.Visibility = Visibility.Collapsed;
                    ReminderSection.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    PasswordSection.Visibility = Visibility.Collapsed;
                    BankCartSection.Visibility = Visibility.Collapsed;
                    ReminderSection.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    PasswordSection.Visibility = Visibility.Collapsed;
                    BankCartSection.Visibility = Visibility.Collapsed;
                    PasportSection.Visibility = Visibility.Collapsed;
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

        private void txtSecurityCodeCollapsed_TextChanged(object sender, TextChangedEventArgs e) => txtSecutityCode.Password = txtSecutityCodeCollapsed.Text;

        private void cbHide_Checked(object sender, RoutedEventArgs e)
        {
            if (cbHide.IsChecked != null && (bool)cbHide.IsChecked)
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
            if (cbHideSecurityCode.IsChecked != null && (bool)cbHideSecurityCode.IsChecked)
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
                FileName = "http://" + _accountModel.WebSite,
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
                    case 0: Clipboard.SetText(_accountModel.Username); break;
                    case 1: Clipboard.SetText(_accountModel.Password); break;
                    case 2: Clipboard.SetText(_accountModel.WebSite); break;
                    case 3: Clipboard.SetText(_accountModel.Username); break;
                    case 4: Clipboard.SetText(_accountModel.SecurityCode); break;
                    case 5: Clipboard.SetText(_accountModel.Username); break;
                    case 6: Clipboard.SetText(_accountModel.PassportNumber); break;
                    case 7: Clipboard.SetText(_accountModel.PassportPlaceOfIssue); break;
                }
            }
            catch { }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChangedPasswordPage = new ChangedPasswordPage(_accountModel)
            {
                DeleteAccountCallback = DeletedAccount,
                ChangedAccountCallback = ChangedAccount,
            };
            NavigationService?.Navigate(ChangedPasswordPage);
        }

        private void PasswordInformationPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            txtPassword.Password = txtPasswordCollapsed.Text;
            txtSecutityCode.Password = txtSecutityCodeCollapsed.Text;

            cbCustomFolder.SelectedValuePath = "Value";
            cbCustomFolder.DisplayMemberPath = "Key";
            var pairs = App.MainWindow.Model.DatabaseApplication.CustomFolders.ToDictionary(i => i.Name, i => i.ID);
            cbCustomFolder.Items.Add(new KeyValuePair<string, string>((string)FindResource("NotChosen"), null));
            foreach (var i in pairs) cbCustomFolder.Items.Add(i);
        }
    }
}
