using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using OlibKey.Core.Helpers;
using OlibKey.Core.Structures;
using OlibKey.Core.ViewModels;
using PleasantUI;
using PleasantUI.Controls;

namespace OlibKey.Core;

public class OlibKeyApp : Application
{
    private static List<IResourceProvider>? _languageDictionaries;
    private static ResourceDictionary? _currentLocalizationDictionary;
    
    public static PleasantTheme PleasantTheme { get; private set; } = null!;

    public static PleasantWindow MainWindow { get; protected set; } = null!;

    public static ApplicationViewModel ViewModel { get; }

    static OlibKeyApp()
    {
        ViewModel = new ApplicationViewModel();
    }

    public OlibKeyApp()
    {
        DataContext = ViewModel;
    }

    public override void OnFrameworkInitializationCompleted()
    {
        _languageDictionaries = Localization.GetLocalizations((ResourceDictionary?)Current?.FindResource("LanguageDictionaries"));
        
        UpdateLanguage();

        PleasantTheme = new PleasantTheme();
        Styles.Add(PleasantTheme);
    }

    /// <summary>
    /// Gets a localized string by the specified key. If there is no such key in the dictionary, the key itself is returned.
    /// </summary>
    /// <param name="key">Key in the localization dictionary</param>
    /// <returns>Localized string or key itself</returns>
    public static string GetLocalString(string key)
    {
        if (Current!.TryFindResource(key, out object? objectText))
            return objectText as string ?? string.Empty;
        return key;
    }
    
    /// <summary>
    /// Looks for a suitable resource in the program.
    /// </summary>
    /// <param name="key">Resource name</param>
    /// <typeparam name="T">Resource type</typeparam>
    /// <returns>Resource found, otherwise null</returns>
    public static T GetResource<T>(object key)
    {
        object? value = null;

        IResourceHost? current = Current;

        while (current != null)
        {
            if (current is { } host)
            {
                if (host.TryGetResource(key, out value))
                {
                    return (T)value!;
                }
            }

            current = ((IStyleHost)current).StylingParent as IResourceHost;
        }

        return (T)value!;
    }

    /// <summary>
    /// Updates the language in the program. Called after changing the language in the settings.
    /// </summary>
    /// <exception cref="NullReferenceException">Dictionary of languages is null</exception>
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

            // Workaround for implementing dynamic language change when the program was compiled using the AOT compiler
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