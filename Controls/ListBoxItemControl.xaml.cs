using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OlibPasswordManager.Controls
{
    /// <summary>
    /// Логика взаимодействия для ListBoxItemControl.xaml
    /// </summary>
    public partial class ListBoxItemControl : UserControl
    {
        private Properties.Core.User AccountContext { get => App.MainWindow.DataContext as Properties.Core.User; }
        public ListBoxItemControl()
        {
            InitializeComponent();

            if (AccountContext.Type == 1)
            {
                imageIcon.Source = (ImageSource)FindResource("saveDrawingImage");
            }
        }
    }
}
