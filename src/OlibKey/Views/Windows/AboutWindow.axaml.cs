using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OlibKey.Views.Windows
{
	public class AboutWindow : Window
	{
		public AboutWindow() => InitializeComponent();

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);

			this.FindControl<Button>("bClose").Click += (s, e) => Close();
			this.FindControl<Button>("bGitHub").Click += (s, e) => Process.Start(new ProcessStartInfo
			{
				FileName = "https://github.com/MagnificentEagle/OlibKey",
				UseShellExecute = true
			});
			this.FindControl<Button>("bVK").Click += (s, e) => Process.Start(new ProcessStartInfo
			{
				FileName = "https://vk.com/olibkey",
				UseShellExecute = true
			});
			this.FindControl<Button>("bFacebook").Click += (s, e) => Process.Start(new ProcessStartInfo
			{
				FileName = "https://www.facebook.com/olibkey",
				UseShellExecute = true
			});
			this.FindControl<TextBlock>("tbVersion").Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
			this.FindControl<TextBox>("tbInformation").Text = $"{Application.Current.FindResource("AccessText1")}\n{Application.Current.FindResource("AccessText2")}\n\nVersion .NET: {Environment.Version}\nOS: {RuntimeInformation.OSDescription}\nArchitecture: {RuntimeInformation.OSArchitecture}";
		}
	}
}
