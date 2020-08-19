using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OlibKey.Core;

namespace OlibKey.Views.Windows
{
	public class PasswordGeneratorWindow : Window
	{
		public TextBox _tbPassword;
		public Button _saveButton;

		public PasswordGeneratorWindow() => InitializeComponent();

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			_tbPassword = this.FindControl<TextBox>("tbPassword");
			_saveButton = this.FindControl<Button>("saveButton");

			DataContext = Program.Settings;
		}
		private void ClickGeneratePassword(object sender, RoutedEventArgs e)
		{
			try
			{
				_tbPassword.Text = PasswordGenerator.RandomPassword();
			}
			catch
			{
				_ = MessageBox.Show(this, null, (string)Application.Current.FindResource("MB7"), (string)Application.Current.FindResource("Error"),
					MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
			}
		}
		private void SavePassword(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(_tbPassword.Text)) _tbPassword.Text = PasswordGenerator.RandomPassword();

			Close(true);
		}
		private void CopyGeneratedPassword(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(_tbPassword.Text)) _tbPassword.Text = PasswordGenerator.RandomPassword();
			Application.Current.Clipboard.SetTextAsync(_tbPassword.Text);
		}
	}
}