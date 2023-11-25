using PleasantUI.Core.Enums;

namespace OlibKey.Core.Structures;

public static class Dictionaries
{
    public static readonly Dictionary<Theme, int> Themes = new()
    {
        { Theme.System, 0 },
        { Theme.Light, 1 },
        { Theme.Dark, 2 }
    };
}