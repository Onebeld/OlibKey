using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using System;
using System.Text.RegularExpressions;

namespace OlibKey.Views.Windows
{
	public class SettingsWindow : Window
	{
		private TabItem _tiStorage;
		private TextBox _tbIteration;
		private TextBox _tbNumberOfEncryptionProcedures;
		private ComboBox _cbTheme;
		private ComboBox _cbLanguage;
		private Button _bClose;

		public SettingsWindow()
		{
			InitializeComponent();
			_cbLanguage.SelectedIndex = App.Settings.Language switch
			{
				"ru-RU" => 1,
				"uk-UA" => 2,
				"de-DE" => 3,
				"fr-FR" => 4,
				"hy-AM" => 5,
				_ => 0
			};

			_cbTheme.SelectedIndex = App.Settings.Theme switch
			{
				"Gloomy" => 1,
				"Mysterious" => 2,
				_ => 0
			};

			_cbTheme.SelectionChanged += ThemeChange;
			_cbLanguage.SelectionChanged += LanguageChange;
			_bClose.Click += (s, e) => Close();
			Closing += (s, e) =>
			{
				if (App.MainWindowViewModel.IsUnlockDatabase)
				{
					Regex reg = new Regex(@"^\d+$");
					if (_tbIteration.Text == "" || !reg.IsMatch(_tbIteration.Text) || _tbIteration.Text == "0"
						|| _tbNumberOfEncryptionProcedures.Text == "" || !reg.IsMatch(_tbNumberOfEncryptionProcedures.Text) ||
						_tbNumberOfEncryptionProcedures.Text == "0")
					{
						_ = MessageBox.Show(this, null, (string)Application.Current.FindResource("CDBError1"), (string)Application.Current.FindResource("Error"),
						MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
						return;
					}

					if (App.MainWindowViewModel.SelectedTabItem != null)
					{
						App.MainWindowViewModel.SelectedTabItem.ViewModel.Iterations = int.Parse(_tbIteration.Text);
						App.MainWindowViewModel.SelectedTabItem.ViewModel.NumberOfEncryptionProcedures = int.Parse(_tbNumberOfEncryptionProcedures.Text);
					}
				}
			};

			_tiStorage.IsEnabled = App.MainWindowViewModel.SelectedTabItem.ViewModel.IsUnlockDatabase;

			if (App.MainWindowViewModel.SelectedTabItem != null)
			{
				_tbIteration.Text = App.MainWindowViewModel.SelectedTabItem.ViewModel.Iterations.ToString();
				_tbNumberOfEncryptionProcedures.Text = App.MainWindowViewModel.SelectedTabItem.ViewModel.NumberOfEncryptionProcedures.ToString();
			}
		}


		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			_cbTheme = this.FindControl<ComboBox>("cbTheme");
			_cbLanguage = this.FindControl<ComboBox>("cbLanguage");
			_bClose = this.FindControl<Button>("bClose");
			_tbIteration = this.FindControl<TextBox>("tbIteration");
			_tbNumberOfEncryptionProcedures = this.FindControl<TextBox>("tbNumberOfEncryptionProcedures");
			_tiStorage = this.FindControl<TabItem>("tiStorage");
		}

		private void LanguageChange(object sender, SelectionChangedEventArgs e)
		{
			App.Settings.Language = _cbLanguage.SelectedIndex switch
			{
				1 => "ru-RU",
				2 => "uk-UA",
				3 => "de-DE",
				4 => "fr-FR",
				5 => "hy-AM",
				_ => "en-US"
			};
			Application.Current.Styles[4] = new StyleInclude(new Uri("resm:Styles?assembly=OlibKey"))
			{
				Source = new Uri($"avares://OlibKey/Assets/Local/lang.{App.Settings.Language}.axaml")
			};
		}

		private void ThemeChange(object sender, SelectionChangedEventArgs e)
		{
			App.Settings.Theme = _cbTheme.SelectedIndex switch
			{
				1 => "Gloomy",
				2 => "Mysterious",
				_ => "Dazzling"
			};

			Application.Current.Styles[2] = new StyleInclude(new Uri("resm:Styles?assembly=OlibKey"))
			{
				Source = new Uri($"avares://OlibKey/Assets/Themes/{App.Settings.Theme}.axaml")
			};
		}
	}
}
