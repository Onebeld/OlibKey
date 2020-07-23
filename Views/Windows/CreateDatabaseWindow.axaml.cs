using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OlibKey.Core;
using System;

namespace OlibKey.Views.Windows
{
	public class CreateDatabaseWindow : Window
	{
		public TextBox TbPassword;
		public TextBox TbPathDatabase;
		public TextBox TbIteration;
		public TextBox TbNumberOfEncryptionProcedures;
		private Button _bSelectPath;
		private Button _bSave;
		private ProgressBar _pbHard;

		public CreateDatabaseWindow()
		{
			InitializeComponent();
			_bSelectPath.Click += SelectPath;
			_bSave.Click += (s, e) => Close(true);
		}

		private async void SelectPath(object sender, RoutedEventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.Filters.Add(new FileDialogFilter { Name = "Olib-Files", Extensions = { "olib" } });
			string res = await dialog.ShowAsync(this);
			if (res != null) TbPathDatabase.Text = res;
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			TbPassword = this.FindControl<TextBox>("tbPassword");
			TbPathDatabase = this.FindControl<TextBox>("tbPathDatabase");
			TbIteration = this.FindControl<TextBox>("tbIteration");
			TbNumberOfEncryptionProcedures = this.FindControl<TextBox>("tbNumberOfEncryptionProcedures");
			_bSelectPath = this.FindControl<Button>("bSelectPath");
			_pbHard = this.FindControl<ProgressBar>("pbHard");
			_bSave = this.FindControl<Button>("bSave");

			_ = TbPassword.GetObservable(TextBox.TextProperty).Subscribe(value => PasswordUtils.DeterminingPasswordComplexity(_pbHard, TbPassword));
		}

		private void CheckedPassword(object sender, RoutedEventArgs e)
		{
			CheckBox cb = (CheckBox)sender;
			TbPassword.PasswordChar = cb.IsChecked == true ? '\0' : '•';
		}
	}
}
