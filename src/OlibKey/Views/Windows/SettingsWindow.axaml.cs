using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using OlibKey.Views.Controls;
using System;
using System.Text.RegularExpressions;

namespace OlibKey.Views.Windows
{
	public class SettingsWindow : Window
	{
		private TabItem _tiStorage;
		private TextBox _tbIteration;
		private TextBox _tbNumberOfEncryptionProcedures;
		private TextBox _tbAutosave;
		private TextBox _tbBlock;
		private TextBox _tbMessage;
		private TextBox _tbDaysDeleteFromTrash;
		private TextBox _tbClearingClipboard;
		private ComboBox _cbTheme;
		private ComboBox _cbLanguage;
		private CheckBox _cbUseCompression;
		private CheckBox _cbUseTrash;

		private int _selectionTheme;

		public SettingsWindow()
		{
			InitializeComponent();
			DataContext = Program.Settings;

			_cbLanguage.SelectedIndex = Program.Settings.Language switch
			{
				"ru-RU" => 1,
				"uk-UA" => 2,
				"de-DE" => 3,
				"fr-FR" => 4,
				"hy-AM" => 5,
				_ => 0
			};

			_cbTheme.SelectedIndex = _selectionTheme = Program.Settings.Theme switch
			{
				"Gloomy" => 1,
				"Mysterious" => 2,
				"Turquoise" => 3,
				"Emerald" => 4,
				_ => 0
			};

			_cbTheme.SelectionChanged += ThemeChange;
			_cbLanguage.SelectionChanged += LanguageChange;
			this.FindControl<Button>("bClose").Click += (s, e) => Close();
			Closing += (s, e) =>
			{
				Regex reg = new Regex(@"^[1-9]\d*$");
				if (!reg.IsMatch(_tbAutosave.Text)
				|| !reg.IsMatch(_tbBlock.Text)
				|| !reg.IsMatch(_tbMessage.Text)
				|| !reg.IsMatch(_tbDaysDeleteFromTrash.Text)
				|| !reg.IsMatch(_tbClearingClipboard.Text))
				{
					_ = MessageBox.Show(this, null, (string)Application.Current.FindResource("CDBError1"), (string)Application.Current.FindResource("Error"),
					MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
					e.Cancel = true;
					return;
				}

				if (App.MainWindowViewModel.IsUnlockDatabase)
				{
					if (!reg.IsMatch(_tbIteration.Text)
					    || !reg.IsMatch(_tbNumberOfEncryptionProcedures.Text))
					{
						_ = MessageBox.Show(this, null, (string)Application.Current.FindResource("CDBError1"), (string)Application.Current.FindResource("Error"),
						MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
						e.Cancel = true;
						return;
					}

					if (App.MainWindowViewModel.SelectedTabItem != null)
					{
						((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.Iterations = int.Parse(_tbIteration.Text);
						((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.NumberOfEncryptionProcedures = int.Parse(_tbNumberOfEncryptionProcedures.Text);
						((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.UseCompression = _cbUseCompression.IsChecked ?? false;
						((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.UseTrash = _cbUseTrash.IsChecked ?? false;
					}
				}

				Program.Settings.AutosaveDuration = int.Parse(_tbAutosave.Text);
				Program.Settings.BlockDuration = int.Parse(_tbBlock.Text);
				Program.Settings.MessageDuration = int.Parse(_tbMessage.Text);
				Program.Settings.DaysAfterDeletion = int.Parse(_tbDaysDeleteFromTrash.Text);
				Program.Settings.TimeToClearTheClipboard = int.Parse(_tbClearingClipboard.Text);

				App.Autosave.Stop();

				App.Autosave.Interval = new TimeSpan(0, Program.Settings.AutosaveDuration, 0);
				App.Autoblock.Interval = new TimeSpan(0, Program.Settings.BlockDuration, 0);

				App.Autosave.Start();
			};

			_tiStorage.IsEnabled = App.MainWindowViewModel.SelectedTabItem != null && ((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.IsUnlockDatabase;

			if (App.MainWindowViewModel.SelectedTabItem != null)
			{
				_tbIteration.Text = ((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.Iterations.ToString();
				_tbNumberOfEncryptionProcedures.Text = ((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.NumberOfEncryptionProcedures.ToString();
				_cbUseCompression.IsChecked = ((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.UseCompression;
				_cbUseTrash.IsChecked = ((DatabaseControl)App.MainWindowViewModel.SelectedTabItem.Content).ViewModel.UseTrash;
			}

			_tbAutosave.Text = Program.Settings.AutosaveDuration.ToString();
			_tbBlock.Text = Program.Settings.BlockDuration.ToString();
			_tbMessage.Text = Program.Settings.MessageDuration.ToString();
			_tbDaysDeleteFromTrash.Text = Program.Settings.DaysAfterDeletion.ToString();
			_tbClearingClipboard.Text = Program.Settings.TimeToClearTheClipboard.ToString();
		}


		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			_cbTheme = this.FindControl<ComboBox>("cbTheme");
			_cbLanguage = this.FindControl<ComboBox>("cbLanguage");
			_tbIteration = this.FindControl<TextBox>("tbIteration");
			_tbNumberOfEncryptionProcedures = this.FindControl<TextBox>("tbNumberOfEncryptionProcedures");
			_tiStorage = this.FindControl<TabItem>("tiStorage");
			_tbAutosave = this.FindControl<TextBox>("tbAutosave");
			_tbBlock = this.FindControl<TextBox>("tbBlock");
			_tbMessage = this.FindControl<TextBox>("tbMessage");
			_cbUseCompression = this.FindControl<CheckBox>("cbUseCompression");
			_tbDaysDeleteFromTrash = this.FindControl<TextBox>("tbDaysDeleteFromTrash");
			_cbUseTrash = this.FindControl<CheckBox>("cbUseTrash");
			_tbClearingClipboard = this.FindControl<TextBox>("tbClearingClipboard");
		}

		private void LanguageChange(object sender, SelectionChangedEventArgs e)
		{
			Program.Settings.Language = _cbLanguage.SelectedIndex switch
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
				Source = new Uri($"avares://OlibKey/Assets/Local/lang.{Program.Settings.Language}.axaml")
			};

            int theme = _selectionTheme;

            _cbTheme.SelectedIndex = -1;
            _cbTheme.SelectedIndex = theme;
		}

		private void ThemeChange(object sender, SelectionChangedEventArgs e)
		{
			Program.Settings.Theme = _cbTheme.SelectedIndex switch
			{
				1 => "Gloomy",
				2 => "Mysterious",
				3 => "Turquoise",
				4 => "Emerald",
				_ => "Dazzling"
			};

			_selectionTheme = _cbTheme.SelectedIndex;

			Application.Current.Styles[2] = new StyleInclude(new Uri("resm:Styles?assembly=OlibKey"))
			{
				Source = new Uri($"avares://OlibKey/Assets/Themes/{Program.Settings.Theme}.axaml")
			};
		}
	}
}