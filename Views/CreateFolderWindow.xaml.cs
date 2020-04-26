using OlibKey.AccountStructures;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OlibKey.Views
{
    /// <summary>
    /// Логика взаимодействия для CreateFolderWindow.xaml
    /// </summary>
    public partial class CreateFolderWindow : Window
    {
        public CreateFolderWindow()
        {
            InitializeComponent();
        }
        public CreateFolderWindow(CustomFolder custom)
        {
            InitializeComponent();
            customFolder = custom;
            DataContext = custom;
        }

        private CustomFolder customFolder;

        private void Drag(object sender, MouseButtonEventArgs e) => DragMove();

        private void TbMasterPassword_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
                Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Storyboard_Completed(object sender, EventArgs e) => Close();

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
