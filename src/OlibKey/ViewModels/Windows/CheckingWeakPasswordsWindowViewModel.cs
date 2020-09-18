using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using OlibKey.Core;
using OlibKey.Structures;
using OlibKey.Views.Controls;
using OlibKey.Views.Windows;
using ReactiveUI;

namespace OlibKey.ViewModels.Windows
{
    public class CheckingWeakPasswordsWindowViewModel : ReactiveObject
    {
        private ObservableCollection<LoginListItem> _loginList = new ObservableCollection<LoginListItem>();

        private double _overallComplexity;

        #region ReactiveCommand's

        private ReactiveCommand<Unit, Unit> CloseWindowCommand { get; }

        #endregion

        #region Property's

        private ObservableCollection<LoginListItem> LoginList
        {
            get => _loginList; set => this.RaiseAndSetIfChanged(ref _loginList, value);
        }
        private double OverallComplexity
        {
            get => _overallComplexity;
            set => this.RaiseAndSetIfChanged(ref _overallComplexity, value);
        }

        #endregion

        public CheckingWeakPasswordsWindowViewModel()
        {
            CloseWindowCommand = ReactiveCommand.Create(() => { App.MainWindowViewModel.CheckingWindow.Close(); });

            for (int index = 0; index < App.MainWindowViewModel.SelectedTabItem.LoginList.Count; index++)
            {
                LoginListItem item = App.MainWindowViewModel.SelectedTabItem.LoginList[index];
                if (item.LoginItem.Type == 0)
                    if (PasswordUtils.CheckPasswordStrength(item.LoginItem.Password) < 200)
                        Add(item.LoginItem, item.IconLogin, item.LoginID);
            }

            double sum = 0;

            sum += App.MainWindowViewModel.SelectedTabItem.LoginList.Where(item => item.LoginItem.Type == 0)
                .Aggregate<LoginListItem, double>(0, (current, item) => current + (PasswordUtils.CheckPasswordStrength(item.LoginItem.Password) > 300
                    ? 300
                    : PasswordUtils.CheckPasswordStrength(item.LoginItem.Password)));

            int count = App.MainWindowViewModel.SelectedTabItem.LoginList.Count(item => item.LoginItem.Type == 0);

            OverallComplexity = count == 0 ? 0 : sum / count;
        }

        private void Add(Login a, Image i, string id) =>
            LoginList.Add(new LoginListItem(a)
            {
                LoginID = id,
                IconLogin = { Source = i.Source },
                SelectedItem = { IsVisible = true },
                IsFavorite = { IsVisible = false }
            });

        private void SelectAll()
        {
            for (int index = 0; index < LoginList.Count; index++) LoginList[index].SelectedItem.IsChecked = true;
        }

        private async void ChangeWeakPassword()
        {
            try
            {
                for (int index = 0; index < LoginList.Count; index++)
                {
                    LoginListItem i = LoginList[index];
                    if (i.SelectedItem.IsChecked ?? false)
                    {
                        foreach (LoginListItem item in App.MainWindowViewModel.SelectedTabItem.LoginList.Where(
                            item => item.LoginID == i.LoginID))
                        {
                            item.LoginItem.Password = PasswordGenerator.RandomPassword();
                            break;
                        }
                    }
                }
                await MessageBox.Show(App.MainWindowViewModel.CheckingWindow, null, (string)Application.Current.FindResource("Successfully"), (string)Application.Current.FindResource("Message"),
                    MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Information);
                App.MainWindowViewModel.CheckingWindow.Close();
            }
            catch
            {
                await MessageBox.Show(App.MainWindowViewModel.CheckingWindow, null, (string)Application.Current.FindResource("MB7"), (string)Application.Current.FindResource("Error"),
                    MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
            }
        }
    }
}
