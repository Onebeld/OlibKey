using Newtonsoft.Json;
using OlibKey.Structures;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OlibKey
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public new static MainWindow MainWindow;
        private static ResourceDictionary ResourceTheme;
        public static Setting Setting;

        private static object sync = new object();

        private static List<CultureInfo> Languages => new List<CultureInfo>();

        private void App_LanguageChanged(object sender, EventArgs e)
        {
            Lang.Default.DefaultLanguage = Language;
            Lang.Default.Save();
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
            Setting = File.Exists("settings.json") ? JsonConvert.DeserializeObject<Setting>(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Roaming\\OlibKey\\settings.json")) : new Setting();

            Language = Lang.Default.IsFirstLanguage ? CultureInfo.CurrentCulture : Lang.Default.DefaultLanguage;
            ResourceTheme = Resources.MergedDictionaries[2];

            if (Setting.Theme != null)
            {
                ResourceTheme.Source = new Uri($"/Properties/Themes/{Setting.Theme}.xaml", UriKind.Relative);
            }

            MainWindow = new MainWindow();
            MainWindow.Show();

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
                if (Lang.Default.IsFirstLanguage)
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

                    Lang.Default.IsFirstLanguage = false;
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
                               where d.Source != null && d.Source.OriginalString.StartsWith("/Localization/lang.")
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
