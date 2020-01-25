using System;
using System.Windows;
using System.Windows.Controls;
using OlibPasswordManager.Properties.Core;
using OlibPasswordManager.Windows;

namespace OlibPasswordManager.Pages
{
    /// <summary>
    /// Логика взаимодействия для ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword
    {
        public ChangePassword() => InitializeComponent();

        private void ButtonCancel(object sender, RoutedEventArgs e) => NavigationService?.GoBack();

        private void OpenPasswordGeneration(object sender, RoutedEventArgs e)
        {
            PasswordGenerator generator = new PasswordGenerator {SaveButton = {Visibility = Visibility.Visible}};
            var b = generator.ShowDialog();
            if (b != null && (bool) b) TxtPassword.Password = generator.TxtPassword.Text;
        }

        private void ChangedPassword(object sender, RoutedEventArgs e)
        {
            User.UsersList[User.IndexUser].Name = TxtName.Text;
            User.UsersList[User.IndexUser].PasswordName = TxtNameAccount.Text;
            User.UsersList[User.IndexUser].Password = TxtPassword.Password;
            User.UsersList[User.IndexUser].Note = TxtNote.Text;
            User.UsersList[User.IndexUser].TimeChanged = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy");

            App.MainWindow.PasswordList.ItemsSource = null;
            App.MainWindow.PasswordList.ItemsSource = User.UsersList;

            App.MainWindow.PasswordList.SelectedIndex = User.IndexUser;

            NavigationService?.GoBack();
        }

        private void CollapsedPassword(object sender, RoutedEventArgs e)
        {
            if (CbHide.IsChecked != null && (bool) CbHide.IsChecked)
            {
                TxtPassword.Visibility = Visibility.Collapsed;
                TxtPasswordCollapsed.Text = TxtPassword.Password;
                TxtPasswordCollapsed.Visibility = Visibility.Visible;
            }
            else if (CbHide.IsChecked != null && !(bool) CbHide.IsChecked)
            {
                TxtPassword.Visibility = Visibility.Visible;
                TxtPasswordCollapsed.Visibility = Visibility.Collapsed;
                TxtPasswordCollapsed.Text = string.Empty;
            }
        }
        private void TxtPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CbHide.IsChecked != null && (bool) CbHide.IsChecked) TxtPassword.Password = TxtPasswordCollapsed.Text;
        }

        private void DeletePassword(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show((string) Application.Current.Resources["MB2"],
                    (string) Application.Current.Resources["Message"], MessageBoxButton.YesNo,
                    MessageBoxImage.Information) == MessageBoxResult.Yes)
                User.UsersList.RemoveAt(App.MainWindow.PasswordList.SelectedIndex);

            App.MainWindow.PasswordList.ItemsSource = null;
            App.MainWindow.PasswordList.ItemsSource = User.UsersList;

            NavigationService?.Navigate(new Uri("/Pages/StartScreen.xaml", UriKind.Relative));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TxtName.Text = User.UsersList[User.IndexUser].Name;
            TxtNameAccount.Text = User.UsersList[User.IndexUser].PasswordName;
            TxtPassword.Password = User.UsersList[User.IndexUser].Password;
            TxtWebSite.Text = User.UsersList[User.IndexUser].WebSite;
            TxtNote.Text = User.UsersList[User.IndexUser].Note;

            TxtCardName.Text = User.UsersList[User.IndexUser].CardName;
            TxtCardNumber.Text = User.UsersList[User.IndexUser].PasswordName;
            TxtDate.Text = User.UsersList[User.IndexUser].DateCard;
            TxtSecurityCode.Password = User.UsersList[User.IndexUser].SecurityCode;

            switch (User.UsersList[User.IndexUser].Type)
            {
                case 0:
                    BUsername.Visibility = Visibility.Collapsed;
                    BPassword.Visibility = Visibility.Collapsed;
                    BWebSite.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    BCardName.Visibility = Visibility.Collapsed;
                    BCardNumber.Visibility = Visibility.Collapsed;
                    BDate.Visibility = Visibility.Collapsed;
                    BSecurityCode.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox b = (PasswordBox) sender;
            PbHard.Value = PasswordUtils.CheckPasswordStrength(b.Password);
        }

        private void PbHard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => ItemControls.ColorProgressBar(PbHard);

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

        private void TxtSecurityCodeCollapsed_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (CbSecurityCodeHide.IsChecked != null && (bool)CbSecurityCodeHide.IsChecked) TxtSecurityCode.Password = TxtSecurityCodeCollapsed.Text;
        }
    }
}
