using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;

namespace OlibKey.Views.Windows
{
	public class SettingsWindow : Window
	{
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

			_cbTheme.SelectedIndex = App.Settings.Theme == "Dark" ? 1 : 0;

			_cbTheme.SelectionChanged += ThemeChange;
			_cbLanguage.SelectionChanged += LanguageChange;
			_bClose.Click += (s, e) => Close();
		}


		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			_cbTheme = this.FindControl<ComboBox>("cbTheme");
			_cbLanguage = this.FindControl<ComboBox>("cbLanguage");
			_bClose = this.FindControl<Button>("bClose");
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
				0 => "Light",
				1 => "Dark",
				_ => "Light"
			};

			Application.Current.Styles[2] = new StyleInclude(new Uri("resm:Styles?assembly=OlibKey"))
			{
				Source = new Uri($"avares://OlibKey/Assets/Themes/{App.Settings.Theme}.axaml")
			};
		}
	}
}
