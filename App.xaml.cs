using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using OlibPasswordManager.Properties.Core;

namespace OlibPasswordManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainWindow MainWindow;
        public static Settings Settings;

        public static List<CultureInfo> Languages { get; } = new List<CultureInfo>();
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
            {
                Settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("settings.json"));
            }
            if (GlobalSettings.Default.GlobalFirstLang)
            {
                Language = CultureInfo.CurrentCulture;
            }
            else
            {
                Language = GlobalSettings.Default.GlobalLanguage;
            }

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
                if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

                //1. Меняем язык приложения:
                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                //2. Создаём ResourceDictionary для новой культуры
                ResourceDictionary dict = new ResourceDictionary();
                if (GlobalSettings.Default.GlobalFirstLang)
                {
                    try
                    {
                        dict.Source = new Uri(string.Format("/Properties/Localization/lang.{0}.xaml", CultureInfo.CurrentCulture.ToString()), UriKind.Relative);
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
                        dict.Source = new Uri(string.Format("/Properties/Localization/lang.{0}.xaml", value.Name), UriKind.Relative);
                    }
                    catch
                    {
                        dict.Source = new Uri("/Properties/Localization/lang.xaml", UriKind.Relative);
                    }
                }

                //3. Находим старую ResourceDictionary и удаляем его и добавляем новую ResourceDictionary
                ResourceDictionary oldDict = (from d in Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("/Properties/Localization/lang.")
                                              select d).FirstOrDefault();
                if (oldDict != null)
                {
                    int ind = Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Current.Resources.MergedDictionaries.Remove(oldDict);
                    Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                {
                    Current.Resources.MergedDictionaries.Add(dict);
                }

                //4. Вызываем евент для оповещения всех окон.
                LanguageChanged(Current, new EventArgs());
            }
        }
    }
}
