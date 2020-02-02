using OlibPasswordManager.Properties.Core;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace OlibPasswordManager.Pages
{
    /// <summary>
    /// Логика взаимодействия для PasswordInformation.xaml
    /// </summary>
    public partial class PasswordInformation
    {
        public PasswordInformation() => InitializeComponent();

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

        private void ChangedPassword(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new Uri("/Pages/ChangePassword.xaml", UriKind.Relative));

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
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
            if (CbSecurityCodeHide.IsChecked != null && (bool) CbSecurityCodeHide.IsChecked)
            {
                TxtSecurityCode.Visibility = Visibility.Collapsed;
                TxtSecurityCodeCollapsed.Text = TxtPassword.Password;
                TxtSecurityCodeCollapsed.Visibility = Visibility.Visible;
            }
            else if (CbSecurityCodeHide.IsChecked != null && !(bool) CbSecurityCodeHide.IsChecked)
            {
                TxtSecurityCode.Visibility = Visibility.Visible;
                TxtSecurityCodeCollapsed.Visibility = Visibility.Collapsed;
                TxtSecurityCodeCollapsed.Text = string.Empty;
            }
        }
    }
}
