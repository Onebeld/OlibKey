using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using OlibPasswordManager.Properties.Core;

namespace OlibPasswordManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings
    {
        private bool _isFirst = true;
        private bool _isFirstTheme = true;

        public Settings()
        {
            InitializeComponent();
            App.LanguageChanged += LanguageChanged;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CbTheme.SelectedValuePath = "Key";
            CbTheme.DisplayMemberPath = "Value";
            KeyValuePair<string, string>[] valuePair = {
                new KeyValuePair<string, string>("Light", (string)FindResource("Light")),
                new KeyValuePair<string, string>("Dark", (string)FindResource("Dark"))
            };
            foreach (KeyValuePair<string, string> i in valuePair) CbTheme.Items.Add(i);
            CbTheme.SelectedIndex = Additional.GlobalSettings.ApplyTheme != null
                    ? valuePair.ToList().FindIndex(i => i.Key == Additional.GlobalSettings.ApplyTheme)
                    : 0;
            CbLang.SelectedValuePath = "Key";
            CbLang.DisplayMemberPath = "Value";
            var valuePair1 = new[]
            {
                new KeyValuePair<string, string>("en-US", "English"),
                new KeyValuePair<string, string>("ru-RU", "Русский"),
                new KeyValuePair<string, string>("uk-UA", "Український"),
                new KeyValuePair<string, string>("de-DE", "Deutsch"),
                new KeyValuePair<string, string>("fr-FR", "Français"),
                new KeyValuePair<string, string>("hy-AM", "Հայերեն")
            };
            foreach (var i in valuePair1) CbLang.Items.Add(i);

            cbCollapsedWindow.IsChecked = Additional.GlobalSettings.CollapseOnClose;

            CbLang.SelectedIndex = valuePair1.ToList().FindIndex(i => i.Key == GlobalSettings.Default.GlobalLanguage.Name);
        }

        private void cbLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isFirst) App.Language = new CultureInfo(CbLang.SelectedValue.ToString());
            _isFirst = false;
        }

        private void LanguageChanged(object sender, EventArgs e)
        {
            GlobalSettings.Default.GlobalLanguage = App.Language;
            GlobalSettings.Default.Save();
        }

        private void Button_Click(object sender, RoutedEventArgs e) => Close();
        private void CbTheme_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isFirstTheme)
            {
                Additional.GlobalSettings.ApplyTheme = CbTheme.SelectedValue.ToString();

                var dict = new ResourceDictionary
                {
                    Source = new Uri($"/Properties/Themes/{Additional.GlobalSettings.ApplyTheme}.xaml", UriKind.Relative)
                };

                var oldDict = (from d in Application.Current.Resources.MergedDictionaries
                    where d.Source != null && d.Source.OriginalString.StartsWith("/Properties/Themes/")
                    select d).FirstOrDefault();
                if (oldDict != null)
                {
                    int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                            Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else Application.Current.Resources.MergedDictionaries.Add(dict);
            }

            _isFirstTheme = false;
        }

        private void Settings_OnClosing(object sender, CancelEventArgs e)
        {
            if (cbCollapsedWindow.IsChecked != null)
                Additional.GlobalSettings.CollapseOnClose = (bool) cbCollapsedWindow.IsChecked;

            File.WriteAllText("settings.json", JsonConvert.SerializeObject(Additional.GlobalSettings));
        }
    }
}
