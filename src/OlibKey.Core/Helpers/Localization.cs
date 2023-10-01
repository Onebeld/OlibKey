using Avalonia.Controls;
using OlibKey.Core.Structures;

namespace OlibKey.Core.Helpers;

public static class Localization
{
    public static readonly Language[] Languages =
    {
        new("English (English)", "en"),
    };

    public static string GetDictionaryLanguageKey(ResourceDictionary languageDictionary)
    {
        if (languageDictionary.TryGetResource("LanguageKey", null, out object? value) && value is string languageKey)
            return languageKey;
        throw new KeyNotFoundException();
    }

    public static List<IResourceProvider> GetLocalizations(ResourceDictionary? languageDictionary)
    {
        if (languageDictionary is null)
            throw new NullReferenceException();
        
        IList<IResourceProvider> resourceProviders = languageDictionary.MergedDictionaries;
        return new List<IResourceProvider>(resourceProviders);
    }
}