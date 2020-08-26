using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OlibKey.ViewModels.Windows;

namespace OlibKey.Views.Windows
{
    public class TrashWindow : Window
    {
        public TrashWindowViewModel ViewModel;

        public TrashWindow() => InitializeComponent();

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = ViewModel = new TrashWindowViewModel();
        }
    }
}
