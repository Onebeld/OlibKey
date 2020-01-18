using OlibPasswordManager.Properties.Core;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OlibPasswordManager.Pages
{
    /// <summary>
    /// Логика взаимодействия для PasswordInformation.xaml
    /// </summary>
    public partial class PasswordInformation : Page
    {
        public PasswordInformation() => InitializeComponent();

        #region CopyText
        private void CopyPassword(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(txtPassword.Password);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(txtNameAccount.Text);
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(txtWebSite.Text);
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(txtCardNumber.Text);
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(txtSecurityCode.Password);
        }
        #endregion

        private void CollapsedPassword(object sender, RoutedEventArgs e)
        {
            if ((bool)cbHide.IsChecked)
            {
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordCollapsed.Text = txtPassword.Password;
                txtPasswordCollapsed.Visibility = Visibility.Visible;
            }
            else if (!(bool)cbHide.IsChecked)
            {
                txtPassword.Visibility = Visibility.Visible;
                txtPasswordCollapsed.Visibility = Visibility.Collapsed;
                txtPasswordCollapsed.Text = null;
            }
        }

        private void ChangedPassword(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new Uri("/Pages/ChangePassword.xaml", UriKind.Relative));

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtName.Text = User.UsersList[User.IndexUser].Name;
            txtNameAccount.Text = User.UsersList[User.IndexUser].PasswordName;
            txtPassword.Password = User.UsersList[User.IndexUser].Password;
            txtWebSite.Text = User.UsersList[User.IndexUser].WebSite;
            labelCreateData.Content = User.UsersList[User.IndexUser].TimeCreate;
            labelChangeData.Content = User.UsersList[User.IndexUser].TimeChanged;
            txtNote.Text = User.UsersList[User.IndexUser].Note;

            txtCardName.Text = User.UsersList[User.IndexUser].CardName;
            txtCardNumber.Text = User.UsersList[User.IndexUser].PasswordName;
            txtDate.Text = User.UsersList[User.IndexUser].DateCard;
            txtSecurityCode.Password = User.UsersList[User.IndexUser].SecurityCode;

            brNote.Visibility = txtNote.Text == "" ? Visibility.Collapsed : Visibility.Visible;
            bWebSite.Visibility = txtWebSite.Text == "" ? Visibility.Collapsed : Visibility.Visible;

            txtLabelChange.Visibility = User.UsersList[User.IndexUser].TimeChanged == null ? Visibility.Collapsed : Visibility.Visible;

            switch (User.UsersList[User.IndexUser].Type)
            {
                case 0:
                    bCardName.Visibility = Visibility.Collapsed;
                    bCardNumber.Visibility = Visibility.Collapsed;
                    bDate.Visibility = Visibility.Collapsed;
                    bSecurityCode.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    bUsername.Visibility = Visibility.Collapsed;
                    bPassword.Visibility = Visibility.Collapsed;
                    bWebSite.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "http://" + txtWebSite.Text,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e) => pbHard.Value = PasswordUtils.CheckPasswordStrength(txtPassword.Password);

        private void pbHard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => ItemControls.ColorProgressBar(pbHard);
    }
}
