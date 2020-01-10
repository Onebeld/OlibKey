using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About() => InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Random r = new Random();

            Version.Content = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Author.Content = r.Next(0, 50) < 1 ? "Дмитрий Мирзоян" : "Дмитрий Жутков";
        }

        private void Button_Click(object sender, RoutedEventArgs e) => Close();

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://vk.com/olibpasswordmanager",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://github.com/MagnificentEagle/OlibPasswordManager",
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
