using System;
using System.Windows;

namespace OlibKey
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Action CloseProgramCallback { get; set; }
        private void CloseProgram() => CloseProgramCallback?.Invoke();
        public MainWindow() => InitializeComponent();

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CloseProgram();
            e.Cancel = true;
        }
    }
}
