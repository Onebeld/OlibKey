using OlibKey.Core.Helpers;

namespace OlibKey.Core.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Determines whether the source string is considered similar to the search string based on a given similarity threshold
    /// </summary>
    /// <param name="str">Initial string</param>
    /// <param name="searchText">The string by which similarity is measured</param>
    /// <returns><b>true</b> if the similarity percentage between str and searchText is greater than the predefined similarity threshold, otherwise <b>false</b></returns>
    public static bool IsDesiredString(this string? str, string searchText)
    {
        if (str is null) return false;
        
        if (str.Contains(searchText))
            return true;
        
        double percent = StringFinder.CalculateSimilarity(str, searchText);

        return percent > OlibKeySettings.Instance.SearchSimilarity;
    }

    /// <summary>
    /// Converts a valid string URI representative into a <see cref="Uri"/> object
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static Uri ToUri(this string str) => new(str);
}