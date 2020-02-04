using OlibPasswordManager.Properties.Core;
using OlibPasswordManager.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OlibPasswordManager.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreatePassword.xaml
    /// </summary>
    public partial class CreatePassword
    {
        public CreatePassword() => InitializeComponent();

        private void OpenPasswordGeneration(object sender, RoutedEventArgs e)
        {
            var generator = new PasswordGenerator {SaveButton = {Visibility = Visibility.Visible}};
            var b = generator.ShowDialog();
            if (b != null && (bool) b) txtPassword.Password = generator.TxtPassword.Text;
        }


        private void CollapsedPassword(object sender, RoutedEventArgs e)
        {
            if (cbHide.IsChecked != null && (bool)cbHide.IsChecked)
            {
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordCollapsed.Text = txtPassword.Password;
                txtPasswordCollapsed.Visibility = Visibility.Visible;
            }
            else if (cbHide.IsChecked != null && !(bool)cbHide.IsChecked)
            {
                txtPassword.Visibility = Visibility.Visible;
                txtPasswordCollapsed.Visibility = Visibility.Collapsed;
                txtPasswordCollapsed.Text = string.Empty;
            }
        }

        private void TxtPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cbHide.IsChecked != null && (bool)cbHide.IsChecked) txtPassword.Password = txtPasswordCollapsed.Text;
        }

        private void CloseCreatePassword(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new Uri("/Pages/StartScreen.xaml", UriKind.Relative));

        private void SavePasswordInList(object sender, RoutedEventArgs e)
        {
            switch (cbType.SelectedIndex)
            {
                case 0:
                    User.UsersList.Add(new User
                    {
                        Name = txtName.Text,
                        Note = txtNote.Text,
                        Password = txtPassword.Password,
                        PasswordName = txtNameAccount.Text,
                        WebSite = txtWebSite.Text,
                        TimeCreate = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy"),
                        Image = $"http://www.google.com/s2/favicons?domain={txtWebSite.Text}",
                        Type = cbType.SelectedIndex
                    });
                    break;
                case 1:
                    User.UsersList.Add(new User
                    {
                        Name = txtName.Text,
                        Note = txtNote.Text,
                        PasswordName = txtCardNumber.Text,
                        TimeCreate = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy"),
                        CardName = txtCardName.Text,
                        DateCard = txtDate.Text,
                        SecurityCode = TxtSecurityCode.Password,
                        Type = cbType.SelectedIndex
                    });
                    break;
                case 2:
                    User.UsersList.Add(new User
                    {
                        Name = txtName.Text,
                        TimeCreate = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy"),
                        Note = txtNote.Text,
                        PasswordName = txtPassportName.Text,
                        PassportNumber = txtPassportNumber.Text,
                        PassportPlaceOfIssue = txtPassportPlaceOfIssue.Text,
                        Type = cbType.SelectedIndex
                    });
                    break;
            }


            App.MainWindow.PasswordList.ItemsSource = null;
            App.MainWindow.PasswordList.ItemsSource = User.UsersList;

            App.MainWindow.PasswordListNotifyIcon.ItemsSource = null;
            App.MainWindow.PasswordListNotifyIcon.ItemsSource = User.UsersList;

            NavigationService?.Navigate(new Uri("/Pages/StartScreen.xaml", UriKind.Relative));
        }
        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox b = (PasswordBox) sender;
            pbHard.Value = PasswordUtils.CheckPasswordStrength(b.Password);
        }

        private void PbHard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => ItemControls.ColorProgressBar(pbHard);

        private void CbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbType.SelectedIndex)
            {
                case 0:
                    bCardName.Visibility = Visibility.Collapsed;
                    bCardNumber.Visibility = Visibility.Collapsed;
                    bDate.Visibility = Visibility.Collapsed;
                    bSecurityCode.Visibility = Visibility.Collapsed;
                    bUsername.Visibility = Visibility.Visible;
                    bPassword.Visibility = Visibility.Visible;
                    bWebSite.Visibility = Visibility.Visible;
                    bPassportName.Visibility = Visibility.Collapsed;
                    bPassportNumber.Visibility = Visibility.Collapsed;
                    bPassportPlaceOfIssue.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    bCardName.Visibility = Visibility.Visible;
                    bCardNumber.Visibility = Visibility.Visible;
                    bDate.Visibility = Visibility.Visible;
                    bSecurityCode.Visibility = Visibility.Visible;
                    bUsername.Visibility = Visibility.Collapsed;
                    bPassword.Visibility = Visibility.Collapsed;
                    bWebSite.Visibility = Visibility.Collapsed;
                    bPassportName.Visibility = Visibility.Collapsed;
                    bPassportNumber.Visibility = Visibility.Collapsed;
                    bPassportPlaceOfIssue.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    bCardName.Visibility = Visibility.Collapsed;
                    bCardNumber.Visibility = Visibility.Collapsed;
                    bDate.Visibility = Visibility.Collapsed;
                    bSecurityCode.Visibility = Visibility.Collapsed;
                    bUsername.Visibility = Visibility.Collapsed;
                    bPassword.Visibility = Visibility.Collapsed;
                    bWebSite.Visibility = Visibility.Collapsed;
                    bPassportName.Visibility = Visibility.Visible;
                    bPassportNumber.Visibility = Visibility.Visible;
                    bPassportPlaceOfIssue.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) => cbType.SelectedIndex = 0;

        private void CollapsedSecurityCode(object sender, RoutedEventArgs e)
        {
            if (CbSecurityCodeHide.IsChecked != null && (bool)CbSecurityCodeHide.IsChecked)
            {
                TxtSecurityCode.Visibility = Visibility.Collapsed;
                TxtSecurityCodeCollapsed.Text = txtPassword.Password;
                TxtSecurityCodeCollapsed.Visibility = Visibility.Visible;
            }
            else if (cbHide.IsChecked != null && !(bool)cbHide.IsChecked)
            {
                TxtSecurityCode.Visibility = Visibility.Visible;
                TxtSecurityCodeCollapsed.Visibility = Visibility.Collapsed;
                TxtSecurityCodeCollapsed.Text = string.Empty;
            }
        }
        private void TxtSecurityCode_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (CbSecurityCodeHide.IsChecked != null && (bool)CbSecurityCodeHide.IsChecked)
                TxtSecurityCode.Password = TxtSecurityCodeCollapsed.Text;
        }
    }
}
