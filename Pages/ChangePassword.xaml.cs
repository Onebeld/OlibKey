using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        private void ButtonCancel(object sender, RoutedEventArgs e) => NavigationService.GoBack();

        private void OpenPasswordGeneration(object sender, RoutedEventArgs e)
        {
            PasswordGenerator generator = new PasswordGenerator();
            generator.saveButton.Visibility = Visibility.Visible;
            if ((bool)generator.ShowDialog()) txtPassword.Password = generator.txtPassword.Text;
        }

        private void ChangedPassword(object sender, RoutedEventArgs e)
        {
            User.UsersList[User.IndexUser].Name = txtName.Text;
            User.UsersList[User.IndexUser].PasswordName = txtNameAccount.Text;
            User.UsersList[User.IndexUser].Password = txtPassword.Password;
            User.UsersList[User.IndexUser].Note = txtNote.Text;
            User.UsersList[User.IndexUser].TimeChanged = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy");

            App.MainWindow.PasswordList.ItemsSource = null;
            App.MainWindow.PasswordList.ItemsSource = User.UsersList;

            App.MainWindow.PasswordList.SelectedIndex = User.IndexUser;

            NavigationService.GoBack();
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
            if ((bool)cbHide.IsChecked) txtPassword.Password = txtPasswordCollapsed.Text;
        }

        private void DeletePassword(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить элемент?", "Сообщение", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                User.UsersList.RemoveAt(App.MainWindow.PasswordList.SelectedIndex);
            }

            App.MainWindow.PasswordList.ItemsSource = null;
            App.MainWindow.PasswordList.ItemsSource = User.UsersList;

            NavigationService.Navigate(new Uri("/Pages/StartScreen.xaml", UriKind.Relative));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtName.Text = User.UsersList[User.IndexUser].Name;
            txtNameAccount.Text = User.UsersList[User.IndexUser].PasswordName;
            txtPassword.Password = User.UsersList[User.IndexUser].Password;
            txtWebSite.Text = User.UsersList[User.IndexUser].WebSite;
            txtNote.Text = User.UsersList[User.IndexUser].Note;
        }
    }
}
