using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using OlibKey.Structures;
using OlibKey.ViewModels.Windows;
using OlibKey.Views.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Primitives;
using OlibKey.Controls.ColorPicker;
using OlibKey.ViewModels.Color;
using Avalonia.Media;

namespace OlibKey.Views.Windows
{
    public class SearchWindow : ReactiveWindow<SearchWindowViewModel>
    {
        public SearchWindowViewModel SearchViewModel { get; set; }

        private TextBox _tbSearchText;

        private RadioButton _rLogin;
        private RadioButton _rBankCard;
        private RadioButton _rPassport;
        private RadioButton _rReminder;
        private RadioButton _rNotes;
        private RadioButton _rAll;

        private CheckBox _cbUseColor;

        private ToggleButton _tbSortAlphabetically;
        private ToggleButton _tbFavorite;
        private ToggleButton _tbActiveReminder;

        public ColorPicker colorPicker;

        public Popup pColorPicker;

        private ListBox _lbFolders;

        private ArgbColorViewModel _argbColorViewModel;

        public SearchWindow()
        {
            AvaloniaXamlLoader.Load(this);
            InitializeComponent();
        }

        private async void InitializeComponent()
        {
            _rLogin = this.FindControl<RadioButton>("rLogin");
            _rBankCard = this.FindControl<RadioButton>("rBankCard");
            _rPassport = this.FindControl<RadioButton>("rPassport");
            _rReminder = this.FindControl<RadioButton>("rReminder");
            _rNotes = this.FindControl<RadioButton>("rNotes");
            _rAll = this.FindControl<RadioButton>("rAll");
            _tbSearchText = this.FindControl<TextBox>("tbSearchText");
            _lbFolders = this.FindControl<ListBox>("lbFolders");
            _tbSortAlphabetically = this.FindControl<ToggleButton>("tbSortAlphabetically");
            _tbFavorite = this.FindControl<ToggleButton>("tbFavorite");
            _tbActiveReminder = this.FindControl<ToggleButton>("tbActiveReminder");
            pColorPicker = this.FindControl<Popup>("pColorPicker");
            colorPicker = this.FindControl<ColorPicker>("colorPicker");
            _cbUseColor = this.FindControl<CheckBox>("cbUseColor");


            DataContext = SearchViewModel = new SearchWindowViewModel { ChangeItemCallback = CloseColorPicker };
            _rAll.IsChecked = true;
            _lbFolders.PointerPressed += (_, __) => ViewModel.SelectedFolderItem = null;
            Closed += SearchWindow_Closed;
            await Task.Delay(50);
            _tbSearchText.GetObservable(TextBox.TextProperty).Subscribe(_ => ClearListAndSearchElement());


            colorPicker.ChangeColor += _colorPicker_ChangeColor;
        }

        private void CommandCheckingUseColor(object sender, RoutedEventArgs e) => _cbUseColor.IsChecked = !_cbUseColor.IsChecked;

        private void CloseColorPicker() => pColorPicker.Close();

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
            App.MainWindowViewModel.SelectedTabItem.Database.Folders =
                SearchViewModel.FolderList.Select(item => item.DataContext as Folder).ToList();

        private async void ClearListAndSearchElement()
        {
            SearchViewModel.LoginList.Clear();

            await Task.Delay(1); // needed because the list does not correctly handle found items

            List<LoginListItem> selectedItemList = _rLogin.IsChecked ?? false
                ? App.MainWindowViewModel.SelectedTabItem.LoginList.ToList()
                    .FindAll(x => x.LoginItem.Type == 0)
                : _rBankCard.IsChecked ?? false
                ? App.MainWindowViewModel.SelectedTabItem.LoginList.ToList()
                    .FindAll(x => x.LoginItem.Type == 1)
                : _rPassport.IsChecked ?? false
                ? App.MainWindowViewModel.SelectedTabItem.LoginList.ToList()
                    .FindAll(x => x.LoginItem.Type == 2)
                : _rReminder.IsChecked ?? false
                ? App.MainWindowViewModel.SelectedTabItem.LoginList.ToList()
                    .FindAll(x => x.LoginItem.Type == 3)
                : _rNotes.IsChecked ?? false
                ? App.MainWindowViewModel.SelectedTabItem.LoginList.ToList()
                    .FindAll(x => x.LoginItem.Type == 4)
                : App.MainWindowViewModel.SelectedTabItem.LoginList.ToList();

            if (SearchViewModel.SelectedFolderItem != null)
                selectedItemList = selectedItemList.FindAll(x =>
                    x.LoginItem.FolderID == ((Folder)SearchViewModel.SelectedFolderItem.DataContext)?.ID);

            if (_tbActiveReminder.IsChecked ?? false)
                selectedItemList = selectedItemList.FindAll(x => x.LoginItem.IsReminderActive);

            if (_tbFavorite.IsChecked ?? false)
                selectedItemList = selectedItemList.FindAll(x => x.LoginItem.Favorite);

            if (!string.IsNullOrEmpty(SearchViewModel.SearchText))
                selectedItemList = selectedItemList.FindAll(x =>
                    x.LoginItem.Name.ToLower().Contains(SearchViewModel.SearchText.ToLower()));

            if (_tbSortAlphabetically.IsChecked ?? false)
                selectedItemList = selectedItemList.OrderBy(x => x.LoginItem.Name).ToList();

            for (int i = 0; i < selectedItemList.Count; i++)
                SearchViewModel.LoginList.Add(new LoginListItem(selectedItemList[i].LoginItem)
                {
                    LoginID = selectedItemList[i].LoginID,
                    IconLogin = { Source = selectedItemList[i].IconLogin.Source },
                    IsFavorite = { IsEnabled = false }
                });
        }

        private void FoldersSelectionChanged(object sender, SelectionChangedEventArgs e) => ClearListAndSearchElement();
        private void Checking(object sender, RoutedEventArgs e) => ClearListAndSearchElement();
    }
}
