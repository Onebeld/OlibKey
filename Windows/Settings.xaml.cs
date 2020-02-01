using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using OlibPasswordManager.Properties.Core;

namespace OlibPasswordManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings
    {
        private bool _isFirst = true;

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
                new KeyValuePair<string, string>("Light", "Светлая"),
                new KeyValuePair<string, string>("Dark", "Темная")
            };
            foreach (KeyValuePair<string, string> i in valuePair) CbTheme.Items.Add(i);

            CbTheme.SelectedIndex = valuePair.ToList().FindIndex(i => i.Key == Additional.GlobalSettings.ApplyTheme);

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

        private void CbTheme_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Additional.GlobalSettings.ApplyTheme = CbTheme.SelectedValue.ToString();
            Application.Current.Resources.MergedDictionaries[3].Source = new Uri($"/Themes/{Additional.GlobalSettings.ApplyTheme}.xaml", UriKind.Relative);
        }
    }
}
