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
        public CreatePassword()
        {
            InitializeComponent();
        }

        private void OpenPasswordGeneration(object sender, RoutedEventArgs e)
        {
            PasswordGenerator generator = new PasswordGenerator();
            generator.saveButton.Visibility = Visibility.Visible;
            if ((bool)generator.ShowDialog())
            {
                txtPassword.Password = generator.txtPassword.Text;
            }
        }

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

        private void txtPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((bool)cbHide.IsChecked)
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
            NavigationService.Navigate(new Uri("/Pages/StartScreen.xaml", UriKind.Relative));
        }

        private void SavePasswordInList(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.UriSource = new Uri($"http://www.google.com/s2/favicons?domain={txtWebSite.Text}");
            bitmap.EndInit();
            if (cbType.SelectedIndex == 0)
            {
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
            }
            else if (cbType.SelectedIndex == 1)
            {
                User.UsersList.Add(new User
                {
                    Name = txtName.Text,
                    Note = txtNote.Text,
                    Icon = (DrawingImage)Application.Current.Resources["bank_cardDrawingImage"],
                    TimeCreate = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy"),
                    CardName = txtCardName.Text,
                    Number = txtCardNumber.Text,
                    DateCard = txtDate.Text,
                    SecurityCode = txtSecurityCode.Password,
                    Type = cbType.SelectedIndex
                });
            }


            App.MainWindow.PasswordList.ItemsSource = null;
            App.MainWindow.PasswordList.ItemsSource = User.UsersList;

            NavigationService.Navigate(new Uri("/Pages/StartScreen.xaml", UriKind.Relative));
        }
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            pbHard.Value = PasswordUtils.CheckPasswordStrength(txtPassword.Password);
        }

        private void pbHard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (pbHard.Value < 100)
            {
                pbHard.Foreground = new SolidColorBrush(Color.FromRgb(235, 20, 0));
            }
            else if (pbHard.Value < 200)
            {
                pbHard.Foreground = new SolidColorBrush(Color.FromRgb(235, 235, 0));
            }
            else
            {
                pbHard.Foreground = new SolidColorBrush(Color.FromRgb(20, 235, 0));
            }
        }

        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbType.SelectedIndex == 0)
            {
                bCardName.Visibility = Visibility.Collapsed;
                bCardNumber.Visibility = Visibility.Collapsed;
                bDate.Visibility = Visibility.Collapsed;
                bSecurityCode.Visibility = Visibility.Collapsed;
                bUsername.Visibility = Visibility.Visible;
                bPassword.Visibility = Visibility.Visible;
                bWebSite.Visibility = Visibility.Visible;
            }
            else if (cbType.SelectedIndex == 1)
            {
                bCardName.Visibility = Visibility.Visible;
                bCardNumber.Visibility = Visibility.Visible;
                bDate.Visibility = Visibility.Visible;
                bSecurityCode.Visibility = Visibility.Visible;
                bUsername.Visibility = Visibility.Collapsed;
                bPassword.Visibility = Visibility.Collapsed;
                bWebSite.Visibility = Visibility.Collapsed;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cbType.SelectedIndex = 0;
        }
    }
}
