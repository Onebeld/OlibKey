using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OlibPasswordManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private bool IsFirst = true;

        public Settings()
        {
            InitializeComponent();
            App.LanguageChanged += LanguageChanged;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbLang.SelectedValuePath = "Key";
            cbLang.DisplayMemberPath = "Value";
            KeyValuePair<string, string>[] valuePair1 = new[]
            {
                new KeyValuePair<string, string>("en-US", "English"),
                new KeyValuePair<string, string>("ru-RU", "Русский"),
                new KeyValuePair<string, string>("uk-UA", "Український"),
                new KeyValuePair<string, string>("de-DE", "Deutsch"),
                new KeyValuePair<string, string>("fr-FR", "Français")
            };
            foreach (KeyValuePair<string, string> i in valuePair1)
            {
                cbLang.Items.Add(i);
            }

            cbLang.SelectedIndex = valuePair1.ToList().FindIndex(i => i.Key == GlobalSettings.Default.GlobalLanguage.Name);
        }

        private void cbLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsFirst)
            {
                App.Language = new CultureInfo(cbLang.SelectedValue.ToString());
                //File.WriteAllText("settings.json", JsonConvert.SerializeObject(App.Settings));
                //int x = Application.ResourceAssembly.Location.Length - 4;
                //Process.Start(Application.ResourceAssembly.Location.Substring(0, x) + ".exe");
                //Application.Current.Shutdown();
            }
            IsFirst = false;
        }

        private void LanguageChanged(object sender, EventArgs e)
        {
            GlobalSettings.Default.GlobalLanguage = App.Language;
            GlobalSettings.Default.Save();
        }
    }
}
