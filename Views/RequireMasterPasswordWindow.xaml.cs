using OlibKey.Core;
using OlibKey.ModelViews;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OlibKey.Views
{
    /// <summary>
    /// Логика взаимодействия для RequireMasterPasswordWindow.xaml
    /// </summary>
    public partial class RequireMasterPasswordWindow : Window
    {
        public Action LoadStorageCallback { get; set; }

        private void Timeline_OnCompleted(object sender, EventArgs e) => Close();
        private void Drag(object sender, MouseButtonEventArgs e) => DragMove();
        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            await Animations.ClosingWindowAnimation(this, ScaleWindow);
            Close();
        }

        public RequireMasterPasswordWindow()
        {
            InitializeComponent();
            tbMasterPassword.Focus();
        }

        private async void CancelButton(object sender, RoutedEventArgs e)
        {
            await Animations.ClosingWindowAnimation(this, ScaleWindow);
            Close();
        }

        private async void LoadStorage()
        {
            try
            {
                MainViewModel.MasterPassword = tbMasterPassword.Password;
                LoadStorageCallback?.Invoke();
                await Animations.ClosingWindowAnimation(this, ScaleWindow);
                Close();
            }
            catch
            {
                MainViewModel.MasterPassword = null;
                MessageBox.Show((string)FindResource("MB3"), (string)FindResource("Error"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadStorageButton(object sender, RoutedEventArgs e) => LoadStorage();

        private void TbMasterPassword_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) LoadStorage();
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
