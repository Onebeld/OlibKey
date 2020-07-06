using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Runtime.InteropServices;

namespace OlibKey.Views.Windows
{
	public class AboutWindow : Window
	{
		private TextBox _tbInformation;
		private Button _bClose;

		public AboutWindow()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);

			_tbInformation = this.FindControl<TextBox>("tbInformation");
			_bClose = this.FindControl<Button>("bClose");

			_bClose.Click += (s, e) => Close();
			_tbInformation.Text = $"{Application.Current.FindResource("AccessText1")}\n{Application.Current.FindResource("AccessText2")}\n\nVersion .NET: {Environment.Version}\nOS: {RuntimeInformation.OSDescription}\nArchitecture: {RuntimeInformation.OSArchitecture}";
		}
	}
}
