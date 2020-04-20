using Microsoft.Win32;
using OlibKey.Core;
using OlibKey.ModelViews;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OlibKey.Views
{
    /// <summary>
    /// Логика взаимодействия для CreatePasswordStorageWindow.xaml
    /// </summary>
    public partial class CreatePasswordStorageWindow : Window
    {
        public CreatePasswordStorageWindow() => InitializeComponent();

        private async void CancelButton(object sender, RoutedEventArgs e)
        {
            await Animations.ClosingWindowAnimation(this, ScaleWindow);
            Close();
        }

        private void Drag(object sender, MouseButtonEventArgs e) => DragMove();
        private async void CreateStorageButton(object sender, RoutedEventArgs e)
        {
            MainViewModel.MasterPassword = TxtPasswordCollapsed.Text;
            MainViewModel.PathStorage = TxtPathSelection.Text;
            await Animations.ClosingWindowAnimation(this, ScaleWindow);
            DialogResult = true;
        }

        private void SelectDirectory(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Olib-files (*.olib)|*.olib"
            };
            if ((bool)saveFileDialog.ShowDialog()) TxtPathSelection.Text = saveFileDialog.FileName;
        }

        private void PbHard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => ItemControls.ColorProgressBar(PbHard);

        private void txtPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            PbHard.Value = PasswordUtils.CheckPasswordStrength(TxtPasswordCollapsed.Text);
            if (!TxtPassword.IsSelectionActive)
                TxtPassword.Password = TxtPasswordCollapsed.Text;
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (TxtPassword.IsSelectionActive)
                TxtPasswordCollapsed.Text = TxtPassword.Password;
        }
        private void cbHide_Checked(object sender, RoutedEventArgs e)
        {
            if (CbHide.IsChecked != null && (bool)CbHide.IsChecked)
            {
                TxtPassword.Visibility = Visibility.Collapsed;
                TxtPasswordCollapsed.Visibility = Visibility.Visible;
            }
            else
            {
                TxtPassword.Visibility = Visibility.Visible;
                TxtPasswordCollapsed.Visibility = Visibility.Collapsed;
            }
        }
        private void Timeline_OnCompleted(object sender, EventArgs e) => Close();
    }
}