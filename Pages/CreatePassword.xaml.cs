using OlibPasswordManager.Properties.Core;
using OlibPasswordManager.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace OlibPasswordManager.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreatePassword.xaml
    /// </summary>
    public partial class CreatePassword : Page
    {
        public CreatePassword() => InitializeComponent();

        private void OpenPasswordGeneration(object sender, RoutedEventArgs e)
        {
            PasswordGenerator generator = new PasswordGenerator {saveButton = {Visibility = Visibility.Visible}};
            if ((bool)generator.ShowDialog())
            {
                txtPassword.Password = generator.txtPassword.Text;
            }
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
                txtPasswordCollapsed.Text = null;
            }
        }

        private void txtPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cbHide.IsChecked != null && (bool)cbHide.IsChecked)
            {
                txtPassword.Password = txtPasswordCollapsed.Text;
            }
        }

        private void CloseCreatePassword(object sender, RoutedEventArgs e)
        {
            txtName.Text = null;
            txtNameAccount.Text = null;
            txtPassword.Password = null;
            txtPasswordCollapsed.Text = null;
            txtNote.Text = null;
            txtWebSite.Text = null;
            NavigationService?.Navigate(new Uri("/Pages/StartScreen.xaml", UriKind.Relative));
        }

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
                        SecurityCode = txtSecurityCode.Password,
                        Type = cbType.SelectedIndex
                    });
                    break;
            }


            App.MainWindow.PasswordList.ItemsSource = null;
            App.MainWindow.PasswordList.ItemsSource = User.UsersList;

            NavigationService?.Navigate(new Uri("/Pages/StartScreen.xaml", UriKind.Relative));
        }
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            pbHard.Value = PasswordUtils.CheckPasswordStrength(txtPassword.Password);
        }

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

        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                    break;
                case 1:
                    bCardName.Visibility = Visibility.Visible;
                    bCardNumber.Visibility = Visibility.Visible;
                    bDate.Visibility = Visibility.Visible;
                    bSecurityCode.Visibility = Visibility.Visible;
                    bUsername.Visibility = Visibility.Collapsed;
                    bPassword.Visibility = Visibility.Collapsed;
                    bWebSite.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) => cbType.SelectedIndex = 0;

        private void CollapsedSecurityCode(object sender, RoutedEventArgs e)
        {
            if (cbHide.IsChecked != null && (bool)cbHide.IsChecked)
            {
                txtSecurityCode.Visibility = Visibility.Collapsed;
                txtSecurityCodeCollapsed.Text = txtPassword.Password;
                txtSecurityCodeCollapsed.Visibility = Visibility.Visible;
            }
            else if (cbHide.IsChecked != null && !(bool)cbHide.IsChecked)
            {
                txtSecurityCode.Visibility = Visibility.Visible;
                txtSecurityCodeCollapsed.Visibility = Visibility.Collapsed;
                txtSecurityCodeCollapsed.Text = null;
            }
        }
        private void txtSecurityCode_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (cbHide.IsChecked != null && (bool)cbHide.IsChecked)
            {
                txtSecurityCode.Password = txtSecurityCodeCollapsed.Text;
            }
        }
    }
}
