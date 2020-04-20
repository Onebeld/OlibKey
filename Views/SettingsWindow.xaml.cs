using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace OlibKey.Views
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private bool _isFirst = true;
        private bool _isFirstTheme = true;

        public SettingsWindow() => InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CbTheme.SelectedValuePath = "Key";
            CbTheme.DisplayMemberPath = "Value";
            KeyValuePair<string, string>[] valuePair = {
                new KeyValuePair<string, string>("Light", (string)FindResource("Light")),
                new KeyValuePair<string, string>("Dark", (string)FindResource("Dark"))
            };
            foreach (var i in valuePair) CbTheme.Items.Add(i);
            CbTheme.SelectedIndex = App.Setting.ApplyTheme != null
                    ? valuePair.ToList().FindIndex(i => i.Key == App.Setting.ApplyTheme)
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
            cbCollapsedWindow.IsChecked = App.Setting.CollapseWhenClosing;
            cbAutorun.IsChecked = App.Setting.AutorunApplication;
            CbLang.SelectedIndex = valuePair1.ToList().FindIndex(i => i.Key == Lang.Default.DefaultLanguage.Name);
        }

        private void cbLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isFirst) App.Language = new CultureInfo(CbLang.SelectedValue.ToString());
            _isFirst = false;
        }

        private void LanguageChanged(object sender, EventArgs e)
        {
            Lang.Default.DefaultLanguage = App.Language;
            Lang.Default.Save();
        }

        private void Button_Click(object sender, RoutedEventArgs e) => Close();
        private void CbTheme_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isFirstTheme)
            {
                App.Setting.ApplyTheme = CbTheme.SelectedValue.ToString();

                var dict = new ResourceDictionary
                {
                    Source = new Uri($"/Themes/{App.Setting.ApplyTheme}.xaml", UriKind.Relative)
                };

                var oldDict = (from d in Application.Current.Resources.MergedDictionaries
                               where d.Source != null && d.Source.OriginalString.StartsWith("/Themes/")
                               select d).FirstOrDefault();
                if (oldDict != null)
                {
                    int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                    Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else Application.Current.Resources.MergedDictionaries.Add(dict);



                var dict1 = new ResourceDictionary
                {
                    Source = new Uri($"/IconsAndImages/Icons.xaml", UriKind.Relative)
                };
                var oldDict1 = (from d in Application.Current.Resources.MergedDictionaries
                               where d.Source != null && d.Source.OriginalString.StartsWith("/IconsAndImages/Icons.xaml")
                               select d).FirstOrDefault();
                int ind1 = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict1);
                Application.Current.Resources.MergedDictionaries.Remove(oldDict1);
                Application.Current.Resources.MergedDictionaries.Insert(ind1, dict1);

            }
            _isFirstTheme = false;
        }

        private void Settings_OnClosing(object sender, CancelEventArgs e)
        {
            if (cbCollapsedWindow.IsChecked != null)
                App.Setting.CollapseWhenClosing = (bool)cbCollapsedWindow.IsChecked;
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                              "\\OlibKey\\settings.json", JsonConvert.SerializeObject(App.Setting));
        }

        private void CBAutorun(object sender, RoutedEventArgs e)
        {
            RegistryKey rkApp =
                Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if ((bool) cbAutorun.IsChecked)
            {
                App.Setting.AutorunApplication = (bool) cbAutorun.IsChecked;
                string s = Assembly.GetExecutingAssembly().Location;
                int x1 = s.Length - 4;
                s = "\"" + s.Remove(x1) + ".exe" + "\"";
                rkApp.SetValue("OlibKey", s + " /StartupHide");
            }
            else
            {
                App.Setting.AutorunApplication = (bool) cbAutorun.IsChecked;
                rkApp.DeleteValue("OlibKey", false);
            }
        }
    }
}
