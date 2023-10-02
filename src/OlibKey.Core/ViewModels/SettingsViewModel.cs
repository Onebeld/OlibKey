using Avalonia.Media;
using OlibKey.Core.Helpers;
using OlibKey.Core.Structures;
using PleasantUI;

namespace OlibKey.Core.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    public static List<FontFamily> FontFamilies { get; }
    
    public Language SelectedLanguage
    {
        get => Localization.Languages.First(lang => lang.Key == OlibKeySettings.Instance.Language);
        set
        {
            OlibKeySettings.Instance.Language = value.Key;
            OlibKeyApp.UpdateLanguage();
        }
    }

    public FontFamily SelectedFontFamily
    {
        get => FontFamily.Parse(OlibKeySettings.Instance.FontName);
        set => OlibKeySettings.Instance.FontName = value.Name;
    }

    static SettingsViewModel()
    {
        FontFamilies = new List<FontFamily>(FontManager.Current.SystemFonts.OrderBy(x => x.Name));
    }
}