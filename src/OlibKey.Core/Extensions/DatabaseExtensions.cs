using System.Text;
using System.Text.Json;
using OlibKey.Core.Helpers;
using OlibKey.Core.Structures;

namespace OlibKey.Core.Extensions;

public static class DatabaseExtensions
{
    public static void Save(this Database database, string path, string masterPassword)
    {
        string file = database.Settings.Iterations + "";
        file += (database.Settings.ImageData ?? "none") + ":";

        string databaseJson = database.ToJson();
        string compressedDatabaseJson = Compressor.Compress(databaseJson);
        string encryptedDatabaseJson = Encryptor.EncryptString(compressedDatabaseJson, masterPassword, database.Settings.Iterations);

        file += encryptedDatabaseJson + ":" + database.Settings.UseTrashcan;
        
        File.WriteAllText(path, file);
    }
    
    public static string ToJson(this Database database)
    {
        using MemoryStream memoryStream = new();

        JsonSerializer.Serialize(memoryStream, database, DatabaseGenerationContext.Default.Database);

        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }
}