using OlibKey.AccountStructures;
using OlibKey.Controls;
using OlibKey.Core;
using OlibKey.ModelViews;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OlibKey.Views
{
    /// <summary>
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchViewModel SearchContext { get => DataContext as SearchViewModel; }

        public SearchWindow() => InitializeComponent();
        private bool _mRestoreForDragMove;

        public void SearchAccount()
        {
            SearchContext.ClearAccountsList();

            foreach (var i in App.MainWindow.Model.AccountsList)
            {
                Account account = i.DataContext as Account;
                if ((bool)rLogin.IsChecked && account.TypeAccount == 0)
                {
                    if (SearchContext.SelectedFolderItem == null)
                        if (!string.IsNullOrEmpty(SearchContext.SearchText))
                        {
                            if (account.AccountName.ToLower().Contains(SearchContext.SearchText.ToLower()))
                                Add(account);
                        }
                        else
                            Add(account);
                    else if (account.IDFolder == ((CustomFolder)SearchContext.SelectedFolderItem.DataContext).ID)
                        if (!string.IsNullOrEmpty(SearchContext.SearchText))
                        {
                            if (account.AccountName.ToLower().Contains(SearchContext.SearchText.ToLower()))
                                Add(account);
                        }
                        else
                            Add(account);
                }
                else if ((bool)rBankCard.IsChecked && account.TypeAccount == 1)
                {
                    if (SearchContext.SelectedFolderItem == null)
                        if (!string.IsNullOrEmpty(SearchContext.SearchText))
                        {
                            if (account.AccountName.ToLower().Contains(SearchContext.SearchText.ToLower()))
                                Add(account);
                        }
                        else
                            Add(account);
                    else if (account.IDFolder == ((CustomFolder)SearchContext.SelectedFolderItem.DataContext).ID)
                        if (!string.IsNullOrEmpty(SearchContext.SearchText))
                        {
                            if (account.AccountName.ToLower().Contains(SearchContext.SearchText.ToLower()))
                                Add(account);
                        }
                        else
                            Add(account);
                }
                else if ((bool)rPassport.IsChecked && account.TypeAccount == 2)
                {
                    if (SearchContext.SelectedFolderItem == null)
                        if (!string.IsNullOrEmpty(SearchContext.SearchText))
                        {
                            if (account.AccountName.ToLower().Contains(SearchContext.SearchText.ToLower()))
                                Add(account);
                        }
                        else
                            Add(account);
                    else if (account.IDFolder == ((CustomFolder)SearchContext.SelectedFolderItem.DataContext).ID)
                        if (!string.IsNullOrEmpty(SearchContext.SearchText))
                        {
                            if (account.AccountName.ToLower().Contains(SearchContext.SearchText.ToLower()))
                                Add(account);
                        }
                        else
                            Add(account);
                }
                else if ((bool)rReminder.IsChecked && account.TypeAccount == 3)
                {
                    if (SearchContext.SelectedFolderItem == null)
                        if (!string.IsNullOrEmpty(SearchContext.SearchText))
                        {
                            if (account.AccountName.ToLower().Contains(SearchContext.SearchText.ToLower()))
                                Add(account);
                        }
                        else
                        {
                            Add(account);
                        }
                    else if (account.IDFolder == ((CustomFolder)SearchContext.SelectedFolderItem.DataContext).ID)
                        if (!string.IsNullOrEmpty(SearchContext.SearchText))
                        {
                            if (account.AccountName.ToLower().Contains(SearchContext.SearchText.ToLower()))
                            {
                                Add(account);
                            }
                        }
                        else
                        {
                            Add(account);
                        }
                }
                else if ((bool)rAll.IsChecked)
                {
                    if (SearchContext.SelectedFolderItem == null)
                        if (!string.IsNullOrEmpty(SearchContext.SearchText))
                        {
                            if (account.AccountName.ToLower().Contains(SearchContext.SearchText.ToLower()))
                                Add(account);
                        }
                        else
                            Add(account);
                    else if (account.IDFolder == ((CustomFolder)SearchContext.SelectedFolderItem.DataContext).ID)
                        if (!string.IsNullOrEmpty(SearchContext.SearchText))
                        {
                            if (account.AccountName.ToLower().Contains(SearchContext.SearchText.ToLower()))
                                Add(account);
                        }
                        else
                        {
                            Add(account);
                        }
                }
            }
        }

        private void Add(Account a)
        {
            AccountListItem ali = new AccountListItem
            {
                DataContext = a,
                ShowContentCallback = App.MainWindow.Model.ShowAccountContent,
                EditContentCallback = App.MainWindow.Model.ShowEditAccountWindow
            };

            AddAccount(ali);
        }

        public void AddAccount(AccountListItem account) => SearchContext.AddAccount(account);

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (ResizeMode != ResizeMode.CanResize && ResizeMode != ResizeMode.CanResizeWithGrip) return;

                WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
            else
            {
                MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
                _mRestoreForDragMove = WindowState == WindowState.Maximized;
                DragMove();
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => _mRestoreForDragMove = false;

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_mRestoreForDragMove) return;
            _mRestoreForDragMove = false;

            Point point = PointToScreen(e.MouseDevice.GetPosition(this));

            Left = point.X * 0.5;
            Top = point.Y - 15;

            WindowState = WindowState.Normal;
            try
            {
                DragMove();
            }
            catch
            {
                // ignored
            }
        }

        private void Full(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                FullMenu.SetResourceReference(HeaderedItemsControl.HeaderProperty, "Expand");
                WindowState = WindowState.Normal;
            }
            else
            {
                FullMenu.SetResourceReference(HeaderedItemsControl.HeaderProperty, "Reestablish");
                MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
                WindowState = WindowState.Maximized;
            }
        }

        private void Timeline_OnCompleted(object sender, EventArgs e) => SearchContext.CloseSearch();

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) => SearchAccount();

        private void rLogin_Click(object sender, RoutedEventArgs e) => SearchAccount();

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Setting.EnableFastRendering)
            {
                RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
                RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.LowQuality);
            }
            lbFolders.SelectedIndex = -1;
            rAll.IsChecked = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) => lbFolders.UnselectAll();

        private void lbFolders_SelectionChanged(object sender, SelectionChangedEventArgs e) => SearchAccount();

        private void lbFolders_MouseDown(object sender, MouseButtonEventArgs e) => lbFolders.UnselectAll();

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Animations.ClosingWindowAnimation(this, ScaleWindow);
            SearchContext.CloseSearch();
        }

        private async void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            await Animations.ClosingWindowAnimation(this, ScaleWindow);
            Close();
        }
    }
}
