using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using OlibKey.Core;
using OlibKey.ModelViews;

namespace OlibKey.Views
{
    /// <summary>
    /// Логика взаимодействия для ChangeMasterPasswordWindow.xaml
    /// </summary>
    public partial class ChangeMasterPasswordWindow : Window
    {
        public ChangeMasterPasswordWindow() => InitializeComponent();

        private void Drag(object sender, MouseButtonEventArgs e) => DragMove();

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveAndLoadAccount.LoadFiles(App.Setting.PathStorage, TxtOldPassword.Password);
                MainViewModel.MasterPassword = TxtPassword.Password;

                App.MainWindow.Model.SaveAccount();

                MessageBox.Show((string)Application.Current.Resources["Successfully"], (string)Application.Current.Resources["Message"], MessageBoxButton.OK, MessageBoxImage.Information);
                await Animations.ClosingWindowAnimation(this, ScaleWindow);
                Close();
            }
            catch
            {
                MessageBox.Show((string)Application.Current.Resources["MB3"], (string)Application.Current.Resources["Error"], MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void OldCollapsedPassword(object sender, RoutedEventArgs e)
        {
            if (CbOldHide.IsChecked != null && (bool)CbOldHide.IsChecked)
            {
                TxtOldPassword.Visibility = Visibility.Collapsed;
                TxtOldPasswordCollapsed.Text = TxtPassword.Password;
                TxtOldPasswordCollapsed.Visibility = Visibility.Visible;
            }
            else if (CbOldHide.IsChecked != null && !(bool)CbOldHide.IsChecked)
            {
                TxtOldPassword.Visibility = Visibility.Visible;
                TxtOldPasswordCollapsed.Visibility = Visibility.Collapsed;
                TxtOldPasswordCollapsed.Text = string.Empty;
            }
        }

        private void txtOldPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CbHide.IsChecked != null && (bool)CbHide.IsChecked) TxtOldPassword.Password = TxtOldPasswordCollapsed.Text;
        }

        private void txtPasswordCollapsed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CbHide.IsChecked != null && (bool)CbHide.IsChecked) TxtPassword.Password = TxtPasswordCollapsed.Text;
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e) => PbHard.Value = PasswordUtils.CheckPasswordStrength(TxtPassword.Password);
        private void pbHard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => ItemControls.ColorProgressBar(PbHard);

        private void Timeline_OnCompleted(object sender, EventArgs e) => Close();

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            await Animations.ClosingWindowAnimation(this, ScaleWindow);
            Close();
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Setting.EnableFastRendering)
            {
                RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
                RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.LowQuality);
            }
        }
    }
}