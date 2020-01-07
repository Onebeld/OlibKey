using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using OlibPasswordManager.Pages;
using OlibPasswordManager.Properties.Core;

namespace OlibPasswordManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Pages
        private CreatePassword PasswordPage;
        private PasswordInformation PasswordInformation;
        #endregion
        public MainWindow() => InitializeComponent();

        #region OpenWindow
        private void OpenAboutWindow(object sender, RoutedEventArgs e) => new Windows.About().ShowDialog();
        private void OpenSettingsWindow(object sender, RoutedEventArgs e) => new Windows.Settings().ShowDialog();
        private void OpenPasswordGeneratorWindow(object sender, RoutedEventArgs e) => new Windows.PasswordGenerator().ShowDialog();
        #endregion

        private void ClosedApplication(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void CreatePassword(object sender, RoutedEventArgs e)
        {
            PasswordPage = new CreatePassword();
            frame.NavigationService.Navigate(PasswordPage);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using StreamWriter sw = new StreamWriter("Build.txt");
            sw.Write("1.0.0.21");
        }

        private async void OpenCreateData(object sender, RoutedEventArgs e)
        {
            Windows.CreateData data = new Windows.CreateData();
            if ((bool)data.ShowDialog())
            {
                string dir = data.txtPathSelection.Text + "\\" + data.txtName.Text + ".aes";
                await Task.Delay(500);

                EncryptorPro.FileEncrypt(dir, data.txtPassword.Password);
            }
        }
    }
}
