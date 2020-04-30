using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using System.Text.Json;
using OlibKey.Core;

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
        private void Drag(object sender, MouseButtonEventArgs e) => DragMove();
        private void Timeline_OnCompleted(object sender, EventArgs e) => Close();
        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            await Animations.ClosingWindowAnimation(this, ScaleWindow);
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = App.Setting;

            if (App.Setting.EnableFastRendering) RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
            CbTheme.SelectedValuePath = "Key";
            CbTheme.DisplayMemberPath = "Value";
            KeyValuePair<string, string>[] valuePair = {
                new KeyValuePair<string, string>("Light", (string)FindResource("Light")),
                new KeyValuePair<string, string>("Dark", (string)FindResource("Dark"))
            };
            foreach (var i in valuePair) CbTheme.Items.Add(i);

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

            CbLang.SelectedIndex = valuePair1.ToList().FindIndex(i => i.Key == Lang.Default.DefaultLanguage.Name);
        }

        private void cbLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isFirst) App.Language = new CultureInfo(CbLang.SelectedValue.ToString());
            _isFirst = false;
        }

        private void CbTheme_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isFirstTheme)
            {
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
                              "\\OlibKey\\settings.json", JsonSerializer.Serialize(App.Setting));
        }

        private void CbAutorun(object sender, RoutedEventArgs e)
        {
            var rkApp =
                Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (cbAutorun.IsChecked != null && (bool) cbAutorun.IsChecked)
            {
                var s = Assembly.GetExecutingAssembly().Location;
                var x1 = s.Length - 4;
                s = "\"" + s.Remove(x1) + ".exe" + "\"";
                rkApp.SetValue("OlibKey", s + " /StartupHide");
            }
            else
            {
                rkApp?.DeleteValue("OlibKey", false);
            }
        }

        private void CBFastRender(object sender, RoutedEventArgs e)
        {
            if ((bool)cbFastRendering.IsChecked)
            {
                RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
                RenderOptions.SetEdgeMode(Application.Current.MainWindow, EdgeMode.Aliased);
                RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.LowQuality);
                RenderOptions.SetBitmapScalingMode(Application.Current.MainWindow, BitmapScalingMode.LowQuality);
            }
            else
            {
                RenderOptions.SetEdgeMode(this, EdgeMode.Unspecified);
                RenderOptions.SetEdgeMode(Application.Current.MainWindow, EdgeMode.Unspecified);
                RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.Unspecified);
                RenderOptions.SetBitmapScalingMode(Application.Current.MainWindow, BitmapScalingMode.Unspecified);
            }
        }
    }
}
