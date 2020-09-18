using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Threading;
using OlibKey.Views.Windows;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Reflection;

namespace OlibKey
{
	public class App : Application
	{
		public static DispatcherTimer Autosave { get; set; }
		public static DispatcherTimer Autoblock { get; set; }
		public static DispatcherTimer ClearingClipboard { get; set; }

		private static string _resultCheckUpdate;
		private static string _errorResult;

		public static MainWindow MainWindow { get; set; }
		public static MainWindowViewModel MainWindowViewModel { get; set; }
		public static SearchWindow SearchWindow { get; set; }

		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);

			Current.Styles[2] = !string.IsNullOrEmpty(Program.Settings.Theme)
				? new StyleInclude(new Uri("resm:Styles?assembly=OlibKey"))
				{
					Source = new Uri($"avares://OlibKey/Assets/Themes/{Program.Settings.Theme}.axaml")
				}
				: new StyleInclude(new Uri("resm:Styles?assembly=OlibKey"))
				{
					Source = new Uri("avares://OlibKey/Assets/Themes/Dazzling.axaml")
				};

			Autosave = new DispatcherTimer();
			Autoblock = new DispatcherTimer();
			ClearingClipboard = new DispatcherTimer();

			Autosave.Tick += (_, __) =>
			{
				for (var i = 0; i < MainWindowViewModel.TabItems.Count; i++)
					MainWindowViewModel.SaveDatabase(MainWindowViewModel.TabItems[i]);
			};
			ClearingClipboard.Tick += (_, __) =>
            {
				ClearingClipboard.Stop();
				Current.Clipboard.ClearAsync();
				MainWindow.MessageStatusBar((string)Current.FindResource("ClipboardIsCleared"));
            };

			Autosave.Interval = new TimeSpan(0, Program.Settings.AutosaveDuration, 0);
			Autoblock.Interval = new TimeSpan(0, Program.Settings.BlockDuration, 0);

			if (Program.Settings.FirstRun)
			{
				Program.Settings.Language = $"{CultureInfo.CurrentCulture}";
				Program.Settings.FirstRun = false;
			}

			try
			{
				Current.Styles[4] = new StyleInclude(new Uri("resm:Styles?assembly=OlibKey"))
				{
					Source = new Uri($"avares://OlibKey/Assets/Local/lang.{Program.Settings.Language}.axaml")
				};
			}
			catch
			{
				Program.Settings.Language = null;
				Current.Styles[4] = new StyleInclude(new Uri("resm:Styles?assembly=OlibKey"))
				{
					Source = new Uri($"avares://OlibKey/Assets/Local/lang.en-US.axaml")
				};
			}

			Autosave.Start();

			CheckUpdate(false);
		}

		public static async void CheckUpdate(bool b)
		{
			try
			{
				using WebClient wb = new WebClient();
				wb.DownloadStringCompleted += (_, args) =>
				{
					if (args.Error != null)
					{
						_errorResult = args.Error.ToString();
						return;
					}
					_resultCheckUpdate = args.Result;
				};
				await wb.DownloadStringTaskAsync(new Uri(
					"https://raw.githubusercontent.com/MagnificentEagle/OlibKey/master/ForRepository/version.txt"));
				float latest = float.Parse(_resultCheckUpdate.Replace(".", ""));
				float current =
					float.Parse(Assembly.GetExecutingAssembly().GetName().Version?.ToString().Replace(".", "")!);

				if (!(latest > current) && b)
				{
					await MessageBox.Show(MainWindow,
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
					Process.Start(new ProcessStartInfo
					{
						FileName = "https://github.com/MagnificentEagle/OlibKey/releases",
						UseShellExecute = true
					});
				}
			}
			catch (Exception ex)
			{
				if (b)
				{
					await MessageBox.Show(MainWindow, _errorResult ?? ex.ToString(),
							(string)Current.FindResource("MB5"), (string)Current.FindResource("Error"), MessageBox.MessageBoxButtons.Ok,
							MessageBox.MessageBoxIcon.Error);
				}
			}
		}
	}
}