using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using OlibPasswordManager.Properties.Core;
using OlibPasswordManager.Windows;

namespace OlibPasswordManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public new static MainWindow MainWindow;
        private static ResourceDictionary ResourceTheme;

        private static object sync = new object();

        private static List<CultureInfo> Languages => new List<CultureInfo>();

        private void App_LanguageChanged(object sender, EventArgs e)
        {
            GlobalSettings.Default.GlobalLanguage = Language;
            GlobalSettings.Default.Save();
        }
        public App()
        {
            LanguageChanged += App_LanguageChanged;


            Languages.Clear();
            Languages.Add(new CultureInfo("en-US")); //Neutral
            Languages.Add(new CultureInfo("ru-RU"));
            Languages.Add(new CultureInfo("uk-UA"));
            Languages.Add(new CultureInfo("de-DE"));
            Languages.Add(new CultureInfo("hy-AM"));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            AppSettings.Items = File.Exists("settings.json") ? JsonConvert.DeserializeObject<Properties.Core.Settings>(File.ReadAllText("settings.json")) : new Properties.Core.Settings();
            Language = GlobalSettings.Default.GlobalFirstLang ? CultureInfo.CurrentCulture : GlobalSettings.Default.GlobalLanguage;

            ResourceTheme = Resources.MergedDictionaries[3];

            if (AppSettings.Items.ApplyTheme != null)
            {
                ResourceTheme.Source = new Uri($"/Properties/Themes/{AppSettings.Items.ApplyTheme}.xaml", UriKind.Relative);
            }

            MainWindow = new MainWindow();
            bool startHide = false;
            if (AppSettings.Items.AutorunApplication)
            {
                foreach (var i in e.Args)
                {
                    if (i == "/StartupHide")
                    {
                        startHide = true;
                    }
                }
            }
            if (startHide)
            {
                MainWindow.Show();
                MainWindow.Hide();
            }
            else
            {
                MainWindow.Show();
            }

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

            timer.Tick += MainWindow.TimerAutoSafe;
            timer.Interval = new TimeSpan(0, 2, 0);
            timer.Start();

            MainWindow.CheckUpdate(false);
            if (AppSettings.Items.AppGlobalString != null)
                if (File.Exists(AppSettings.Items.AppGlobalString))
                {
                    MainWindow.Title = $"Olib Password Manager - {Path.GetFileName(AppSettings.Items.AppGlobalString)}";
                    if (!MainWindow.IsUnlockedBase)
                        new RequireMasterPassword().ShowDialog();
                }

            base.OnStartup(e);
        }

        public static event EventHandler LanguageChanged;
        public static CultureInfo Language
        {
            get => System.Threading.Thread.CurrentThread.CurrentUICulture;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (ReferenceEquals(value, System.Threading.Thread.CurrentThread.CurrentUICulture)) return;
                System.Threading.Thread.CurrentThread.CurrentUICulture = value;
                var dict = new ResourceDictionary();
                if (GlobalSettings.Default.GlobalFirstLang)
                {
                    try
                    {
                        dict.Source = new Uri($"/Properties/Localization/lang.{CultureInfo.CurrentCulture}.xaml",
                            UriKind.Relative);
                    }
                    catch
                    {
                        dict.Source = new Uri("/Properties/Localization/lang.xaml", UriKind.Relative);
                    }

                    GlobalSettings.Default.GlobalFirstLang = false;
                }
                else
                {
                    try
                    {
                        dict.Source = new Uri($"/Properties/Localization/lang.{value.Name}.xaml", UriKind.Relative);
                    }
                    catch
                    {
                        dict.Source = new Uri("/Properties/Localization/lang.xaml", UriKind.Relative);
                    }
                }

                var oldDict = (from d in Current.Resources.MergedDictionaries
                    where d.Source != null && d.Source.OriginalString.StartsWith("/Properties/Localization/lang.")
                    select d).FirstOrDefault();
                if (oldDict != null)
                {
                    int ind = Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Current.Resources.MergedDictionaries.Remove(oldDict);
                    Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else Current.Resources.MergedDictionaries.Add(dict);

                LanguageChanged?.Invoke(Current, new EventArgs());
            }
        }

        private void WriteLog(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Путь .\\Log
            string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
            if (!Directory.Exists(pathToLog))
                Directory.CreateDirectory(pathToLog); // Создаем директорию, если нужно
            string filename = Path.Combine(pathToLog, string.Format("{0}_{1:dd.MM.yyy}.log",
            AppDomain.CurrentDomain.FriendlyName, DateTime.Now));
            string fullText = string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] [{1}.{2}()]\n{3}\r\n",
            DateTime.Now, e.Exception.TargetSite.DeclaringType, e.Exception.TargetSite.Name, e.Exception);
            lock (sync)
            {
                File.AppendAllText(filename, fullText, Encoding.GetEncoding("UTF-8"));
            }

            MessageBox.Show((string)FindResource("MB9") + $"\n{e.Exception.Message}", (string)FindResource("Error"), MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
