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

			DataContext = App.Settings;
		}

		private string RandomPassword()
		{
			try
			{
				const string lower = "abcdefghijklmnopqrstuvwxyz";
				const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
				const string number = "0123456789";
				const string special = @"~!@#$%^&*():;[]{}<>,.?/\|";
				string other = App.Settings.GeneratorTextOther;

				string allowed = "";
				string password = "";
				if (App.Settings.GeneratorAllowLowercase) allowed += lower;
				if (App.Settings.GeneratorAllowUppercase) allowed += upper;
				if (App.Settings.GeneratorAllowNumber) allowed += number;
				if (App.Settings.GeneratorAllowSpecial) allowed += special;
				if (App.Settings.GeneratorAllowUnderscore && password.IndexOfAny("_".ToCharArray()) == -1)
				{
					allowed += "_";
					password += "_";
				}
				if (App.Settings.GeneratorAllowOther) allowed += other;
				int minChars = int.Parse(App.Settings.GenerationCount);
				int numChars = Encryptor.RandomInteger(minChars, minChars);
				while (password.Length < numChars)
					password += allowed.Substring(Encryptor.RandomInteger(0, allowed.Length - 1), 1);
				password = RandomizeString(password);
				return password;
			}
			catch
			{
				_ = MessageBox.Show(this, null, (string)Application.Current.FindResource("MB7"), (string)Application.Current.FindResource("Error"),
					MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
				return _tbPassword.Text;
			}
		}
		private void ClickGeneratePassword(object sender, RoutedEventArgs e) => _tbPassword.Text = RandomPassword();
		private static string RandomizeString(string str)
		{
			string result = "";
			while (str.Length > 0)
			{
				int i = Encryptor.RandomInteger(0, str.Length - 1);
				result += str.Substring(i, 1);
				str = str.Remove(i, 1);
			}

			return result;
		}
		private void SavePassword(object sender, RoutedEventArgs e)
		{
			if (_tbPassword.Text == null || _tbPassword.Text.Length < 1) _tbPassword.Text = RandomPassword();

			Close(true);
		}
		private void CopyGeneratedPassword(object sender, RoutedEventArgs e) => Application.Current.Clipboard.SetTextAsync(_tbPassword.Text);
	}
}