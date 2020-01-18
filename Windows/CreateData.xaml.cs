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
            if ((bool)dialog.ShowDialog()) TxtPathSelection.Text = dialog.FileName;
        }

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

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e) => PbHard.Value = PasswordUtils.CheckPasswordStrength(TxtPassword.Password);

        private void pbHard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => ItemControls.ColorProgressBar(PbHard);
    }
}
