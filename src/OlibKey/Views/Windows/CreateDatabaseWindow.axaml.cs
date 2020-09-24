using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OlibKey.Core;
using OlibKey.Views.Controls;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace OlibKey.Views.Windows
{
	public class CreateDatabaseWindow : Window
	{
		public TextBox TbPassword;
		public TextBox TbPathDatabase;
		public TextBox TbIteration;
		public TextBox TbNumberOfEncryptionProcedures;
		public CheckBox CbUseCompression;
		public CheckBox CbUseTrash;
		private ProgressBar _pbHard;

		public CreateDatabaseWindow()
		{
			InitializeComponent();
			this.FindControl<Button>("bSelectPath").Click += SelectPath;
			this.FindControl<Button>("bSave").Click += (s, e) => {
				Regex reg = new Regex(@"^[1-9]\d*$");
				if (!reg.IsMatch(TbIteration.Text)
				    || !reg.IsMatch(TbNumberOfEncryptionProcedures.Text))
				{
					_ = MessageBox.Show(this, null, (string)Application.Current.FindResource("CDBError1"), (string)Application.Current.FindResource("Error"),
					MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
					return;
				}
				if (string.IsNullOrEmpty(TbPassword.Text) || string.IsNullOrEmpty(TbPathDatabase.Text))
				{
					_ = MessageBox.Show(this, null, (string)Application.Current.FindResource("CDBError2"), (string)Application.Current.FindResource("Error"),
					MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
					return;
				}
				if (App.MainWindowViewModel.TabItems.Select(item => (DatabaseControl)item.Content).Any(db => db.ViewModel.PathDatabase == TbPathDatabase.Text))
				{
					_ = MessageBox.Show(this, null, (string)Application.Current.FindResource("CDBError3"), (string)Application.Current.FindResource("Error"),
						MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error);
					return;
				}
				Close(true); 
			};
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			TbPassword = this.FindControl<TextBox>("tbPassword");
			TbPathDatabase = this.FindControl<TextBox>("tbPathDatabase");
			TbIteration = this.FindControl<TextBox>("tbIteration");
			TbNumberOfEncryptionProcedures = this.FindControl<TextBox>("tbNumberOfEncryptionProcedures");
			_pbHard = this.FindControl<ProgressBar>("pbHard");
			CbUseTrash = this.FindControl<CheckBox>("cbUseTrash");
			CbUseCompression = this.FindControl<CheckBox>("cbUseCompression");

			TbPassword.GetObservable(TextBox.TextProperty).Subscribe(value => PasswordUtils.DeterminingPasswordComplexity(_pbHard, value));
		}
		private async void SelectPath(object sender, RoutedEventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.Filters.Add(new FileDialogFilter { Name = (string)Application.Current.FindResource("FileOlib"), Extensions = { "olib" } });
			string res = await dialog.ShowAsync(this);
			if (res != null) TbPathDatabase.Text = res;
		}
	}
}
