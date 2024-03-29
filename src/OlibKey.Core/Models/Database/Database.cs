using System.Text.Json;
using Avalonia.Collections;
using OlibKey.Core.Helpers;
using OlibKey.Core.Models.Database;
using PleasantUI;

namespace OlibKey.Core.Structures;

public class Database : ViewModelBase
{
    private AvaloniaList<Data> _data = new();
    private Trashcan _trashcan = new();
    private DatabaseSettings _settings = null!;

    public AvaloniaList<Data> Data
    {
        get => _data;
        set => RaiseAndSet(ref _data, value);
    }
    
    public Trashcan Trashcan
    {
        get => _trashcan;
        set => RaiseAndSet(ref _trashcan, value);
    }

    public DatabaseSettings Settings
    {
        get => _settings;
        set => RaiseAndSet(ref _settings, value);
    }
    
    public void Save(string path, string masterPassword)
    {
        string encryptJson;
        
        {
            string databaseJson = ToJson();
            string compressedJson = Compressor.Compress(databaseJson);
            encryptJson = Encryptor.EncryptString(compressedJson, masterPassword, Settings.Iterations);
        }

        string file = $"{Settings.Name}:{Settings.Iterations}:{encryptJson}:{Settings.UseTrashcan}:";

        file += Settings.ImageData ?? "none";

        File.WriteAllText(path, file);
    }

    public static Database Load(string path, string masterPassword)
    {
        string file = File.ReadAllText(path);

        string[] split = file.Split(':');

        string name = split[0];
        int iterations = int.Parse(split[1]);
        string encryptString = split[2];
        bool useTrashcan = bool.Parse(split[3]);
        string? imageData = split[4];
        
        if (imageData == "none")
            imageData = null;

        {
            string compressedString = Encryptor.EncryptString(encryptString, masterPassword, iterations);
            file = Compressor.Decompress(compressedString);
        }

        Database database = FromJson(file);

        DatabaseSettings settings = new()
        {
            Iterations = iterations,
            UseTrashcan = useTrashcan,
            Name = name,
            ImageData = imageData,
        };

        database.Settings = settings;

        return database;
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this, DatabaseGenerationContext.Default.Database);
    }

    public static Database FromJson(string json)
    {
        return JsonSerializer.Deserialize(json, DatabaseGenerationContext.Default.Database) ?? throw new NullReferenceException();
    }
}