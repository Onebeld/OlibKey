using Avalonia.Controls;
using OlibKey.Core.Models;

namespace OlibKey.Core.Helpers;

public static class Localization
{
    public static readonly Language[] Languages =
    {
        new("English (English)", "en"),
    };

    /// <summary>
    /// Gets the language key from the dictionary. It is required that there is a key named <b>LanguageKey</b> in the dictionary:
    /// <code>&lt;s:String x:Key="LanguageKey"&gt;en&lt;/s:String&gt;</code>
    /// </summary>
    /// <remarks>This is necessary to implement dynamic localization, because AOT compilation will be performed.</remarks>
    /// <param name="languageDictionary">Language dictionary</param>
    /// <returns>Language key</returns>
    /// <exception cref="KeyNotFoundException"><b>LanguageKey</b> not found in the dictionary</exception>
    public static string GetDictionaryLanguageKey(ResourceDictionary languageDictionary)
    {
        if (languageDictionary.TryGetResource("LanguageKey", null, out object? value) && value is string languageKey)
            return languageKey;
        throw new KeyNotFoundException("LanguageKey not found in the dictionary");
    }

    /// <summary>
    /// Gets localization dictionaries.
    /// </summary>
    /// <remarks>This is necessary to implement dynamic localization, because AOT compilation will be performed.</remarks>
    /// <param name="languageDictionary">Dictionary with localizations</param>
    /// <returns>Localization dictionaries.</returns>
    /// <exception cref="NullReferenceException">The dictionary with localizations is null</exception>
    public static List<IResourceProvider> GetLocalizations(ResourceDictionary? languageDictionary)
    {
        if (languageDictionary is null)
            throw new NullReferenceException("The dictionary with localizations is null");
        
        IList<IResourceProvider> resourceProviders = languageDictionary.MergedDictionaries;
        return new List<IResourceProvider>(resourceProviders);
    }
}