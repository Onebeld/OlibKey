using System;
using System.Collections.Generic;
using System.Globalization;
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
        private bool _isFirst = true;

        public Settings()
        {
            InitializeComponent();
            App.LanguageChanged += LanguageChanged;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbLang.SelectedValuePath = "Key";
            cbLang.DisplayMemberPath = "Value";
            var valuePair1 = new[]
            {
                new KeyValuePair<string, string>("en-US", "English"),
                new KeyValuePair<string, string>("ru-RU", "Русский"),
                new KeyValuePair<string, string>("uk-UA", "Український"),
                new KeyValuePair<string, string>("de-DE", "Deutsch"),
                new KeyValuePair<string, string>("fr-FR", "Français")
            };
            foreach (var i in valuePair1) cbLang.Items.Add(i);

            cbLang.SelectedIndex = valuePair1.ToList().FindIndex(i => i.Key == GlobalSettings.Default.GlobalLanguage.Name);
        }

        private void cbLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isFirst) App.Language = new CultureInfo(cbLang.SelectedValue.ToString());
            _isFirst = false;
        }

        private void LanguageChanged(object sender, EventArgs e)
        {
            GlobalSettings.Default.GlobalLanguage = App.Language;
            GlobalSettings.Default.Save();
        }
    }
}
