using Microsoft.Win32;
using OlibPasswordManager.Properties.Core;
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
using System.Windows.Shapes;

namespace OlibPasswordManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreateData.xaml
    /// </summary>
    public partial class CreateData : Window
    {
        public CreateData() => InitializeComponent();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User.UsersList.Clear();


            App.MainWindow.PasswordList.ItemsSource = null;
            App.MainWindow.PasswordList.ItemsSource = User.UsersList;

            DialogResult = true;
        }

        private void PathSelection(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog { Filter = "Olib-files (*.olib)|*.olib" };
            if ((bool)dialog.ShowDialog()) txtPathSelection.Text = dialog.FileName;
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

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e) => pbHard.Value = PasswordUtils.CheckPasswordStrength(txtPassword.Password);

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
    }
}
