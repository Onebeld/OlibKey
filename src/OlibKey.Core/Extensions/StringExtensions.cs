namespace OlibKey.Core.Extensions;

public static class StringExtensions
{
    public static bool IsRightElement(this string? str, string searchText) => str is not null && str.ToLower().Contains(searchText);
}