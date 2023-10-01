using Avalonia;
using Avalonia.Controls;
using OlibKey.Core.Helpers;
using OlibKey.Core.Structures;
using PleasantUI;

namespace OlibKey.Core;

public class OlibKeyApp : Application
{
    public static PleasantTheme PleasantTheme { get; protected set; } = null!;

    private static List<IResourceProvider>? _languageDictionaries;
    private static ResourceDictionary? _currentLocalizationDictionary;

    public override void OnFrameworkInitializationCompleted()
    {
        _languageDictionaries = Localization.GetLocalizations((ResourceDictionary?)Current?.FindResource("LanguageDictionaries"));
        
        UpdateLanguage();

        PleasantTheme = new PleasantTheme();
        Styles.Add(PleasantTheme);
    }

    public static string GetLocalString(string key)
    {
        if (Current!.TryFindResource(key, out object? objectText))
            return objectText as string ?? string.Empty;
        return key;
    }

    public static void UpdateLanguage()
    {
        Language? language = Localization.Languages.FirstOrDefault(x => x.Key == OlibKeySettings.Instance.Language || x.AdditionalKeys.Any(lang => lang == OlibKeySettings.Instance.Language));

        string? key = language.Value.Key;

        if (string.IsNullOrEmpty(key))
        {
            key = "en";
            OlibKeySettings.Instance.Language = key;
        }

        if (_languageDictionaries is null)
            throw new NullReferenceException();

        foreach (IResourceProvider resourceProvider in _languageDictionaries)
        {
            if (resourceProvider is not ResourceDictionary resourceDictionary) continue;

            string languageKey = Localization.GetDictionaryLanguageKey(resourceDictionary);
            
            if (languageKey != OlibKeySettings.Instance.Language) continue;

            if (_currentLocalizationDictionary is null)
            {
                _currentLocalizationDictionary = resourceDictionary;
                Current?.Resources.MergedDictionaries.Add(_currentLocalizationDictionary);
            }
            else
            {
                Current?.Resources.MergedDictionaries.Remove(_currentLocalizationDictionary);
                _currentLocalizationDictionary = resourceDictionary;
                Current?.Resources.MergedDictionaries.Add(_currentLocalizationDictionary);
            }
        }
    }
}