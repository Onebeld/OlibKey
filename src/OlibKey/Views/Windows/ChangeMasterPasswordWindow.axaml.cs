using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OlibKey.Core;
using OlibKey.Views.Controls;
using System;

namespace OlibKey.Views.Windows
{
	public class ChangeMasterPasswordWindow : Window
	{
		private TextBox _tbNewPassword;
		private ProgressBar _pbHard;

		public ChangeMasterPasswordWindow() => InitializeComponent();

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			_tbNewPassword = this.FindControl<TextBox>("tbNewPassword");
			_pbHard = this.FindControl<ProgressBar>("pbHard");

			_tbNewPassword.GetObservable(TextBox.TextProperty).Subscribe(value => PasswordUtils.DeterminingPasswordComplexity(_pbHard, value));
		}

		private void CloseWindow(object sender, RoutedEventArgs e) => Close();

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SaveAndLoadDatabase.LoadFiles(((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content));
				((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.MasterPassword = _tbNewPassword.Text;

				App.MainWindowViewModel.SaveDatabase((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content);

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
