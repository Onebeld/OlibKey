using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OlibKey.Core;

namespace OlibKey.Views.Windows
{
	public class CreateDatabaseWindow : Window
	{
		public TextBox _tbPassword;
		public TextBox _tbPathDatabase;
		private Button _bSelectPath;
		private Button _bSave;
		private ProgressBar _pbHard;

		public CreateDatabaseWindow()
		{
			InitializeComponent();
			_bSelectPath.Click += SelectPath;
			_bSave.Click += (s, e) =>
			{
				Close(true);
			};
		}

		private async void SelectPath(object sender, RoutedEventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.Filters.Add(new FileDialogFilter { Name = "Olib-Files", Extensions = { "olib" } });
			string res = await dialog.ShowAsync(this);
			if (res != null) _tbPathDatabase.Text = res;
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			_tbPassword = this.FindControl<TextBox>("tbPassword");
			_bSelectPath = this.FindControl<Button>("bSelectPath");
			_pbHard = this.FindControl<ProgressBar>("pbHard");
			_tbPathDatabase = this.FindControl<TextBox>("tbPathDatabase");
			_bSave = this.FindControl<Button>("bSave");

			_ = _tbPassword.GetObservable(TextBox.TextProperty).Subscribe(value => PasswordUtils.DeterminingPasswordComplexity(_pbHard, _tbPassword));
		}

		private void CheckedPassword(object sender, RoutedEventArgs e)
		{
			CheckBox cb = (CheckBox)sender;
			_tbPassword.PasswordChar = cb.IsChecked == true ? '\0' : '•';
		}
	}
}
