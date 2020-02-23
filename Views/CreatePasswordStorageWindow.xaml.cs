using Microsoft.Win32;
using OlibKey.Core;
using OlibKey.ModelViews;
using System.Windows;
using System.Windows.Controls;

namespace OlibKey.Views
{
    /// <summary>
    /// Логика взаимодействия для CreatePasswordStorageWindow.xaml
    /// </summary>
    public partial class CreatePasswordStorageWindow : Window
    {
        public CreatePasswordStorageWindow()
        {
            InitializeComponent();
        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CreateStorageButton(object sender, RoutedEventArgs e)
        {
            MainViewModel.MasterPassword = TxtPasswordCollapsed.Text;
            MainViewModel.PathStorage = TxtPathSelection.Text;
            DialogResult = true;
        }

        private void SelectDirectory(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Olib-files (*.olib)|*.olib"
            };
            if ((bool)saveFileDialog.ShowDialog())
            {
                TxtPathSelection.Text = saveFileDialog.FileName;
            }
        }
        private void txtPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            PbHard.Value = PasswordUtils.CheckPasswordStrength(TxtPasswordCollapsed.Text);
            TxtPassword.Password = TxtPasswordCollapsed.Text;
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (TxtPassword.IsSelectionActive)
                TxtPasswordCollapsed.Text = TxtPassword.Password;
        }
    }
}