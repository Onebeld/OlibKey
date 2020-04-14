using Newtonsoft.Json.Bson;
using OlibKey.AccountStructures;
using OlibKey.Controls;
using OlibKey.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            int index = 0;

            foreach (var i in App.MainWindow.Model.AccountsList)
            {
                Account account = i.DataContext as Account;
                if ((bool)rLogin.IsChecked && account.TypeAccount == 0)
                {
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
                else if ((bool)rBankCard.IsChecked && account.TypeAccount == 1)
                {
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
                else if ((bool)rPassport.IsChecked && account.TypeAccount == 2)
                {
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
                else if ((bool)rReminder.IsChecked && account.TypeAccount == 3)
                {
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
                index++;
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

        public void AddAccount(AccountListItem account)
        {
            SearchContext.AddAccount(account);
        }

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

        private void Timeline_OnCompleted(object? sender, EventArgs e) => Close();

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) => SearchAccount();

        private void rLogin_Click(object sender, RoutedEventArgs e) => SearchAccount();

        private void mainWindow_Loaded(object sender, RoutedEventArgs e) => rAll.IsChecked = true;

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => Close();
    }
}
