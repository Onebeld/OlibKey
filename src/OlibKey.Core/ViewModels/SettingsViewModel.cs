using Avalonia;
using Avalonia.Media;
using OlibKey.Core.Helpers;
using OlibKey.Core.Structures;
using PleasantUI;
using PleasantUI.Core;
using PleasantUI.Core.Enums;
using PleasantUI.Windows;

namespace OlibKey.Core.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    public bool IsNotLoaded = true;
    
    public static List<FontFamily> FontFamilies { get; }
    
    public Language SelectedLanguage
    {
        get => Localization.Languages.First(lang => lang.Key == OlibKeySettings.Instance.Language);
        set
        {
            if (IsNotLoaded) return;
            
            OlibKeySettings.Instance.Language = value.Key;
            OlibKeyApp.UpdateLanguage();
        }
    }

    public FontFamily SelectedFontFamily
    {
        get => FontFamily.Parse(OlibKeySettings.Instance.FontName);
        set => OlibKeySettings.Instance.FontName = value.Name;
    }

    public int SelectedIndexTheme
    {
        get
        {
            return PleasantSettings.Instance.Theme switch
            {
                Theme.Light => 1,
                Theme.Dark => 2,

                _ => 0
            };
        }
        set
        {
            if (IsNotLoaded) return;
            
            PleasantSettings.Instance.Theme = value switch
            {
                1 => Theme.Light,
                2 => Theme.Dark,

                _ => Theme.System
            };
            
            OlibKeyApp.PleasantTheme.UpdateTheme();
        }
    }

    public bool UseAccentColor
    {
        get => !PleasantSettings.Instance.PreferUserAccentColor;
        set
        {
            if (IsNotLoaded) return;
            
            PleasantSettings.Instance.PreferUserAccentColor = !value;
            
            if (PleasantSettings.Instance.PreferUserAccentColor)
                return;

            Color? accent = Application.Current?.PlatformSettings?.GetColorValues().AccentColor1;

            if (accent is null)
                return;
            
            PleasantSettings.Instance.NumericalAccentColor = accent.Value.ToUInt32();
            OlibKeyApp.PleasantTheme.UpdateAccentColors(accent.Value);
        }
    }

    static SettingsViewModel()
    {
        FontFamilies = new List<FontFamily>(FontManager.Current.SystemFonts.OrderBy(x => x.Name));
    }

    public async void ChangeAccentColor()
    {
        Color? color = await ColorPickerWindow.SelectColor(OlibKeyApp.MainWindow, PleasantSettings.Instance.NumericalAccentColor);
        
        if (color is null)
            return;

        PleasantSettings.Instance.NumericalAccentColor = color.Value.ToUInt32();
        OlibKeyApp.PleasantTheme.UpdateAccentColors(color.Value);
    }

    public async void CopyAccentColor()
    {
        await OlibKeyApp.MainWindow.Clipboard?.SetTextAsync(
            $"#{PleasantSettings.Instance.NumericalAccentColor.ToString("x8").ToUpper()}")!;
        
        /*OlibKeyApp.ViewModel.NotificationManager.Show(new Notification(OlibKeyApp.GetLocalString("Information"),
            OlibKeyApp.GetLocalString("ColorCopied"),
            NotificationType.Information,
            TimeSpan.FromSeconds(2)));*/
    }

    public async void PasteAccentColor()
    {
        string? data = await OlibKeyApp.MainWindow.Clipboard?.GetTextAsync()!;

        if (uint.TryParse(data, out uint uintColor))
        {
            PleasantSettings.Instance.NumericalAccentColor = uintColor;
            OlibKeyApp.PleasantTheme.UpdateAccentColors(Color.FromUInt32(uintColor));
        }
        else if (Color.TryParse(data, out Color color))
        {
            PleasantSettings.Instance.NumericalAccentColor = color.ToUInt32();
            OlibKeyApp.PleasantTheme.UpdateAccentColors(color);
        }
    }

    public async void ResetSettings()
    {
        string result = await MessageBox.Show(OlibKeyApp.MainWindow, OlibKeyApp.GetLocalString(""), string.Empty, MessageBoxButtons.ReverseYesNo);
        
        if (result != "Yes") return;

        SelectedIndexTheme = 0;
        
        OlibKeyApp.UpdateLanguage();
        
        RaisePropertyChanged(nameof(SelectedLanguage));
        RaisePropertyChanged(nameof(SelectedFontFamily));
        RaisePropertyChanged(nameof(SelectedIndexTheme));
        
        
    }
}