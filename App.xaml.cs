using Newtonsoft.Json;
using OlibKey.Structures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using OlibKey.ModelViews;
using OlibKey.Views;

namespace OlibKey
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
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
            Setting = File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\OlibKey\\settings.json")
                ? JsonConvert.DeserializeObject<Setting>(File.ReadAllText(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                    "\\OlibKey\\settings.json"))
                : new Setting();

            Language = Lang.Default.IsFirstLanguage ? CultureInfo.CurrentCulture : Lang.Default.DefaultLanguage;

            ResourceTheme = Resources.MergedDictionaries[2];
            if (Setting.ApplyTheme != null)
                ResourceTheme.Source = new Uri($"/Themes/{Setting.ApplyTheme}.xaml", UriKind.Relative);

            MainWindow = new MainWindow();
            bool startHide = false;
            if (Setting.AutorunApplication)
                foreach (var i in e.Args)
                    if (i == "/StartupHide")
                        startHide = true;
            if (startHide)
            {
                MainWindow.Show();
                MainWindow.Hide();
            }
            else
                MainWindow.Show();

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

            timer.Tick += (s, d) => MainWindow.Model.SaveAccount();
            timer.Interval = new TimeSpan(0, 2, 0);
            timer.Start();

            if (!string.IsNullOrEmpty(Setting.PathStorage))
            {
                MainViewModel.PathStorage = Setting.PathStorage;
                MainWindow.Model.NameStorage = Path.GetFileName(App.Setting.PathStorage);
                MainWindow.Model.IsLockStorage = true;

                RequireMasterPasswordWindow passwordWindow = new RequireMasterPasswordWindow
                {
                    LoadStorageCallback = MainWindow.Model.LoadAccounts
                };
                passwordWindow.ShowDialog();
            }
            MainWindow.CheckUpdate(false);

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
                        dict.Source = new Uri($"/Localization/lang.{CultureInfo.CurrentCulture}.xaml",
                            UriKind.Relative);
                    }
                    catch
                    {
                        dict.Source = new Uri("/Localization/lang.xaml", UriKind.Relative);
                    }

                    Lang.Default.IsFirstLanguage = false;
                }
                else
                {
                    try
                    {
                        dict.Source = new Uri($"/Localization/lang.{value.Name}.xaml", UriKind.Relative);
                    }
                    catch
                    {
                        dict.Source = new Uri("/Localization/lang.xaml", UriKind.Relative);
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
            try
            {
                string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                if (!Directory.Exists(pathToLog))
                Directory.CreateDirectory(pathToLog);
                string filename = Path.Combine(pathToLog, $"{AppDomain.CurrentDomain.FriendlyName}_{DateTime.Now:dd.MM.yyy}.log");
                string fullText = $"[{DateTime.Now:dd.MM.yyy HH:mm:ss.fff}] [{e.Exception.TargetSite.DeclaringType}.{e.Exception.TargetSite.Name}()]\n{e.Exception}\r\n";
                lock (sync) File.AppendAllText(filename, fullText, Encoding.GetEncoding("UTF-8"));

                MessageBox.Show((string)FindResource("MB9") + $"\n{e.Exception.Message}", (string)FindResource("Error"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch { }
        }
    }
}
