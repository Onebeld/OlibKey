using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using OlibPasswordManager.Properties.Core;

namespace OlibPasswordManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public new static MainWindow MainWindow;

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


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (File.Exists("settings.json"))
                Additional.GlobalSettings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("settings.json"));
            else Additional.GlobalSettings = new Settings();
            Language = GlobalSettings.Default.GlobalFirstLang ? CultureInfo.CurrentCulture : GlobalSettings.Default.GlobalLanguage;

            MainWindow = new MainWindow();
            MainWindow.Show();
        }

        public static event EventHandler LanguageChanged;
        public static CultureInfo Language
        {
            get => System.Threading.Thread.CurrentThread.CurrentUICulture;
            set
            {
                if (value == null) throw new ArgumentNullException("value");
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
    }
}
