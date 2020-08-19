using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OlibKey.Core;
using System;

namespace OlibKey.Views.Windows
{
	public class ChangeMasterPasswordWindow : Window
	{
		private TextBox _tbOldPassword;
		private TextBox _tbNewPassword;
		private ProgressBar _pbHard;

		public ChangeMasterPasswordWindow() => InitializeComponent();

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			_tbOldPassword = this.FindControl<TextBox>("tbOldPassword");
			_tbNewPassword = this.FindControl<TextBox>("tbNewPassword");
			_pbHard = this.FindControl<ProgressBar>("pbHard");

			_tbNewPassword.GetObservable(TextBox.TextProperty).Subscribe(value => PasswordUtils.DeterminingPasswordComplexity(_pbHard, _tbNewPassword));
		}

		private void CheckedOldPassword(object sender, RoutedEventArgs e) => _tbOldPassword.PasswordChar = ((CheckBox)sender).IsChecked ?? false ? '\0' : '•';
		private void CheckedNewPassword(object sender, RoutedEventArgs e) => _tbNewPassword.PasswordChar = ((CheckBox)sender).IsChecked ?? false ? '\0' : '•';

		private void CloseWindow(object sender, RoutedEventArgs e) => Close();

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SaveAndLoadDatabase.LoadFiles(App.MainWindowViewModel.SelectedTabItem);
				App.MainWindowViewModel.SelectedTabItem.ViewModel.MasterPassword = _tbNewPassword.Text;

				App.MainWindowViewModel.SaveDatabase(App.MainWindowViewModel.SelectedTabItem);

				_ = await MessageBox.Show(this, null, (string)Application.Current.FindResource("Successfully"), (string)Application.Current.FindResource("Message"), MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Information);
				Close();
			}
			catch
			{
				_ = await MessageBox.Show(this, null, (string)Application.Current.FindResource("MB3"), (string)Application.Current.FindResource("Error"), MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
			}
		}
	}
}
