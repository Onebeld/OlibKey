using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OlibPasswordManager.Properties.Core;
using OlibPasswordManager.Windows;

namespace OlibPasswordManager.Pages
{
    /// <summary>
    /// Логика взаимодействия для ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Page
    {
        public ChangePassword() => InitializeComponent();

        private void ButtonCancel(object sender, RoutedEventArgs e) => NavigationService?.GoBack();

        private void OpenPasswordGeneration(object sender, RoutedEventArgs e)
        {
            PasswordGenerator generator = new PasswordGenerator {saveButton = {Visibility = Visibility.Visible}};
            if ((bool)generator.ShowDialog()) txtPassword.Password = generator.txtPassword.Text;
        }

        private void ChangedPassword(object sender, RoutedEventArgs e)
        {
            User.UsersList[User.IndexUser].Name = TxtName.Text;
            User.UsersList[User.IndexUser].PasswordName = TxtNameAccount.Text;
            User.UsersList[User.IndexUser].Password = txtPassword.Password;
            User.UsersList[User.IndexUser].Note = txtNote.Text;
            User.UsersList[User.IndexUser].TimeChanged = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy");
            User.UsersList[User.IndexUser].Image = $"http://www.google.com/s2/favicons?domain={txtWebSite.Text}";

            App.MainWindow.PasswordList.ItemsSource = null;
            App.MainWindow.PasswordList.ItemsSource = User.UsersList;

            App.MainWindow.PasswordList.SelectedIndex = User.IndexUser;

            NavigationService?.GoBack();
        }

        private void CollapsedPassword(object sender, RoutedEventArgs e)
        {
            if (cbHide.IsChecked != null && (bool) cbHide.IsChecked)
            {
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordCollapsed.Text = txtPassword.Password;
                txtPasswordCollapsed.Visibility = Visibility.Visible;
            }
            else if (cbHide.IsChecked != null && !(bool) cbHide.IsChecked)
            {
                txtPassword.Visibility = Visibility.Visible;
                txtPasswordCollapsed.Visibility = Visibility.Collapsed;
                txtPasswordCollapsed.Text = null;
            }
        }

        private void txtPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cbHide.IsChecked != null && (bool) cbHide.IsChecked) txtPassword.Password = txtPasswordCollapsed.Text;
        }

        private void DeletePassword(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show((string) Application.Current.Resources["MB2"],
                    (string) Application.Current.Resources["Message"], MessageBoxButton.YesNo,
                    MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                User.UsersList.RemoveAt(App.MainWindow.PasswordList.SelectedIndex);
            }

            App.MainWindow.PasswordList.ItemsSource = null;
            App.MainWindow.PasswordList.ItemsSource = User.UsersList;

            NavigationService?.Navigate(new Uri("/Pages/StartScreen.xaml", UriKind.Relative));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TxtName.Text = User.UsersList[User.IndexUser].Name;
            TxtNameAccount.Text = User.UsersList[User.IndexUser].PasswordName;
            txtPassword.Password = User.UsersList[User.IndexUser].Password;
            txtWebSite.Text = User.UsersList[User.IndexUser].WebSite;
            txtNote.Text = User.UsersList[User.IndexUser].Note;

            txtCardName.Text = User.UsersList[User.IndexUser].CardName;
            txtCardNumber.Text = User.UsersList[User.IndexUser].PasswordName;
            txtDate.Text = User.UsersList[User.IndexUser].DateCard;
            txtSecurityCode.Password = User.UsersList[User.IndexUser].SecurityCode;

            switch (User.UsersList[User.IndexUser].Type)
            {
                case 0:
                    bUsername.Visibility = Visibility.Collapsed;
                    bPassword.Visibility = Visibility.Collapsed;
                    bWebSite.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    BCardName.Visibility = Visibility.Collapsed;
                    bCardNumber.Visibility = Visibility.Collapsed;
                    bDate.Visibility = Visibility.Collapsed;
                    bSecurityCode.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e) => pbHard.Value = PasswordUtils.CheckPasswordStrength(txtPassword.Password);
        private void pbHard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (pbHard.Value < 100)
            {
                pbHard.Foreground = new SolidColorBrush(Color.FromRgb(196, 20, 3));
            }
            else if (pbHard.Value < 200)
            {
                pbHard.Foreground = new SolidColorBrush(Color.FromRgb(222, 222, 64));
            }
            else
            {
                pbHard.Foreground = new SolidColorBrush(Color.FromRgb(27, 199, 11));
            }
        }
    }
}
