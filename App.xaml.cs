using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Threading;
using OlibKey.Structures;
using OlibKey.Views.Windows;

namespace OlibKey
{
    public class App : Application
    {
	    public static Settings Settings { get; set; }
		public static Database Database { get; set; }
		public static DispatcherTimer Autosave { get; set; }

		private static string ResultCheckUpdate;

		public static MainWindow MainWindow { get; set; }
		public static MainWindowViewModel MainWindowViewModel { get; set; }
		public static SearchWindow SearchWindow { get; set; }

		public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
			Settings = File.Exists(AppDomain.CurrentDomain.BaseDirectory + "settings.json")
				? JsonSerializer.Deserialize<Settings>(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "settings.json"))
				: new Settings();

			Current.Styles[2] = !string.IsNullOrEmpty(Settings.ApplyTheme)
				? new StyleInclude(new Uri("resm:Styles?assembly=OlibKey"))
				{
					Source = new Uri($"avares://OlibKey/Assets/Themes/{Settings.ApplyTheme}.xaml")
				}
				: new StyleInclude(new Uri("resm:Styles?assembly=OlibKey"))
				{
					Source = new Uri("avares://OlibKey/Assets/Themes/Light.xaml")
				};

			Autosave = new DispatcherTimer();

			Autosave.Tick += (s, d) => MainWindowViewModel.SaveDatabase();
			Autosave.Interval = new TimeSpan(0, 2, 0);


			if (Settings.FirstRun)
			{
				Settings.Language = $"{CultureInfo.CurrentCulture}";
				Settings.FirstRun = false;
			}

			try
			{
				Current.Styles[4] = new StyleInclude(new Uri("resm:Styles?assembly=OlibKey"))
				{
					Source = new Uri($"avares://OlibKey/Assets/Local/lang.{Settings.Language}.xaml")
				};
			}
			catch
			{
				Settings.Language = null;
				Current.Styles[4] = new StyleInclude(new Uri("resm:Styles?assembly=OlibKey"))
				{
					Source = new Uri($"avares://OlibKey/Assets/Local/lang.en-US.xaml")
				};
			}

			CheckUpdate(false);
        }

		public static  async void CheckUpdate(bool b)
		{
			try
			{
				using var wb = new WebClient();
				wb.DownloadStringCompleted += (s, args) => ResultCheckUpdate = args.Result;
				await wb.DownloadStringTaskAsync(new Uri(
					"https://raw.githubusercontent.com/MagnificentEagle/OlibKey/master/forRepository/version.txt"));
				var latest = float.Parse(ResultCheckUpdate.Replace(".", ""));
				var current =
					float.Parse(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
				if (!(latest > current) && b)
				{
					await MessageBox.Show(null,
						null, (string)Current.FindResource("MB8"), (string)Current.FindResource("Message"),
						MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Information);
					return;
				}

				if (!(latest > current)) return;
				if (await MessageBox.Show(MainWindow,
					null, (string)Current.FindResource("MB4"), (string)Current.FindResource("Message"),
					MessageBox.MessageBoxButtons.YesNo,
					MessageBox.MessageBoxIcon.Question) == MessageBox.MessageBoxResult.Yes)
				{
					var psi = new ProcessStartInfo
					{
						FileName = "https://github.com/MagnificentEagle/OlibKey/releases", UseShellExecute = true
					};
					Process.Start(psi);
				}
			}
			catch
			{
				if (b)
				{
					await MessageBox.Show(MainWindow, null,
						(string)Current.FindResource("MB5"), (string)Current.FindResource("Error"), MessageBox.MessageBoxButtons.Ok,
						MessageBox.MessageBoxIcon.Error);
				}
			}
		}
	}
}