using System.Diagnostics;
using System.Windows;

namespace OlibPasswordManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About() => InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e) => Version.Content =
            " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        private void Button_Click(object sender, RoutedEventArgs e) => Close();

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo {FileName = "https://vk.com/olibpasswordmanager", UseShellExecute = true};
            Process.Start(psi);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://github.com/MagnificentEagle/OlibPasswordManager", UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
