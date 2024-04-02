using OlibKey.Core.Settings;

namespace OlibKey.Core.Structures;

public struct DatabaseInfo
{
    public string MasterPassword;
    public string Path;

    public DatabaseSettings DatabaseSettings;
}