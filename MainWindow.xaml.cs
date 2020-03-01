using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Windows;

namespace OlibKey
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();
        private string _strResult;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Model.ExitProgramVoid();
            e.Cancel = true;
        }

        public async void CheckUpdate(bool b)
        {
            try
            {
                using var wb = new WebClient();
                wb.DownloadStringCompleted += (s, args) => _strResult = args.Result;
                await wb.DownloadStringTaskAsync(new Uri("https://raw.githubusercontent.com/MagnificentEagle/OlibKey/master/forRepository/version.txt"));
                var latest = float.Parse(_strResult.Replace(".", ""));
                var current = float.Parse(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
                if (!(latest > current) && b)
                {
                    MessageBox.Show((string)Application.Current.FindResource("MB8"),
                        (string)Application.Current.FindResource("Message"), MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }
                if (!(latest > current)) return;
                if (MessageBox.Show((string)Application.Current.FindResource("MB4"),
                        (string)Application.Current.FindResource("Message"), MessageBoxButton.YesNo,
                        MessageBoxImage.Information) != MessageBoxResult.Yes) return;
                var psi = new ProcessStartInfo
                {
                    FileName = "https://github.com/MagnificentEagle/OlibKey/releases",
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch
            {
                if (b)
                {
                    MessageBox.Show((string)Application.Current.FindResource("MB5"),
                        (string)Application.Current.FindResource("Error"), MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
    }
}
