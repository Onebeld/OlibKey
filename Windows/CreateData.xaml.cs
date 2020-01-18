using Microsoft.Win32;
using OlibPasswordManager.Properties.Core;
using System.Windows;

namespace OlibPasswordManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreateData.xaml
    /// </summary>
    public partial class CreateData
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
            var dialog = new SaveFileDialog { Filter = "Olib-files (*.olib)|*.olib" };
            var b = dialog.ShowDialog();
            if (b != null && (bool)b) txtPathSelection.Text = dialog.FileName;
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

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e) => pbHard.Value = PasswordUtils.CheckPasswordStrength(txtPassword.Password);

        private void pbHard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => ItemControls.ColorProgressBar(pbHard);
    }
}
