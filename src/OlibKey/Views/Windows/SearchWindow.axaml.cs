using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using OlibKey.Structures;
using OlibKey.ViewModels.Windows;
using OlibKey.Views.Controls;
using System;
using System.Linq;
using Avalonia.Controls.Primitives;
using OlibKey.Controls.ColorPicker;
using OlibKey.ViewModels.Color;
using Avalonia.Media;

namespace OlibKey.Views.Windows
{
    public class SearchWindow : ReactiveWindow<SearchWindowViewModel>
    {
        public SearchWindowViewModel SearchViewModel { get; set; }

        private CheckBox _cbUseColor;

        public ColorPicker colorPicker;

        public Popup pColorPicker;

        private ArgbColorViewModel _argbColorViewModel;

        public SearchWindow()
        {
            AvaloniaXamlLoader.Load(this);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            pColorPicker = this.FindControl<Popup>("pColorPicker");
            colorPicker = this.FindControl<ColorPicker>("colorPicker");
            _cbUseColor = this.FindControl<CheckBox>("cbUseColor");

            DataContext = SearchViewModel = new SearchWindowViewModel { ChangeIndexCallback = CloseColorPicker };
            this.FindControl<ListBox>("lbFolders").PointerPressed += (_, __) => ViewModel.SelectedFolderIndex = -1;

            Closed += SearchWindow_Closed;
            colorPicker.ChangeColor += _colorPicker_ChangeColor;
        }

        private void CommandCheckingUseColor(object sender, RoutedEventArgs e)
        {
            _cbUseColor.IsChecked = !_cbUseColor.IsChecked;
        }

        private void CloseColorPicker()
        {
            pColorPicker.Close();
        }

        private void OpenColorPicker(object sender, RoutedEventArgs e)
        {
            if (SearchViewModel.SelectedFolderItem.FolderContext.UseColor)
            {
                _argbColorViewModel = new ArgbColorViewModel
                {
                    Hex = SearchViewModel.SelectedFolderItem.FolderContext.Color
                };

                pColorPicker.DataContext = _argbColorViewModel;

                pColorPicker.Open();
            }
        }

        private void CheckedUseColor(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked ?? false)
            {
                if (!SearchViewModel.SelectedFolderItem.FolderContext.UseColor)
                {
                    SearchViewModel.SelectedFolderItem.FolderContext.UseColor = true;
                    SearchViewModel.SelectedFolderItem.FolderContext.Color = ((Color)Application.Current.FindResource("ThemeSelectedControlColor")).ToString();
                    SearchViewModel.SelectedFolderItem.bLabelColor.Background = new SolidColorBrush(ColorHelpers.FromHexColor(((Color)Application.Current.FindResource("ThemeSelectedControlColor")).ToString()));
                }
            }
            else
            {
                if (SearchViewModel.SelectedFolderItem.FolderContext.UseColor)
                {
                    SearchViewModel.SelectedFolderItem.FolderContext.UseColor = false;
                    SearchViewModel.SelectedFolderItem.FolderContext.Color = null;
                    SearchViewModel.SelectedFolderItem.bLabelColor.Background = null;
                }
            }
        }

        private void _colorPicker_ChangeColor(object sender, RoutedEventArgs e)
        {
            SearchViewModel.SelectedFolderItem.FolderContext.Color = _argbColorViewModel.ToHexString();
            SearchViewModel.SelectedFolderItem.bLabelColor.Background = new SolidColorBrush(ColorHelpers.FromHexColor(_argbColorViewModel.ToHexString()));
        }

        private void SearchWindow_Closed(object sender, EventArgs e) =>
            ((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.Database.Folders =
                SearchViewModel.FolderList.Select(item => item.DataContext as Folder).ToList();
    }
}
