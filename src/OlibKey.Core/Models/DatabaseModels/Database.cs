using System.Collections.Specialized;
using System.Text.Json;
using System.Text.Json.Serialization;
using Avalonia.Collections;
using OlibKey.Core.Helpers;
using OlibKey.Core.Models.DatabaseModels.StorageTypes;
using OlibKey.Core.Settings;
using PleasantUI;

namespace OlibKey.Core.Models.DatabaseModels;

public class Database : ViewModelBase
{
    private AvaloniaList<Data> _data = [];
    private Trashcan _trashcan = new();
    private DatabaseSettings _settings = null!;
    
    private AvaloniaList<Tag> _tags = [];

    public AvaloniaList<Data> Data
    {
        get => _data;
        set
        {
            RaiseAndSet(ref _data, value);
            
            value.CollectionChanged += DataOnCollectionChanged;
        }
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

    [JsonIgnore]
    public int LoginCount => Data.OfType<Login>().Count();
    
    [JsonIgnore]
    public int BankCardCount => Data.OfType<BankCard>().Count();
    
    [JsonIgnore]
    public int PersonalDataCount => Data.OfType<PersonalData>().Count();
    
    [JsonIgnore]
    public int NotesCount => Data.OfType<Note>().Count();
    
    [JsonIgnore]
    public AvaloniaList<Tag> Tags
    {
        get => _tags;
        private set => RaiseAndSet(ref _tags, value);
    }

    private void DataOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => UpdateInfo();

    private void UpdateInfo()
    {
        RaisePropertyChanged(nameof(LoginCount));
        RaisePropertyChanged(nameof(BankCardCount));
        RaisePropertyChanged(nameof(PersonalDataCount));
        RaisePropertyChanged(nameof(NotesCount));
        
        Tags.Clear();
        
        IEnumerable<IGrouping<string, string>> tags = Data.SelectMany(x => x.Tags).GroupBy(tag => tag);

        foreach (IGrouping<string,string> grouping in tags) 
            Tags.Add(new Tag(grouping.Key, grouping.Count()));
        
        Tags = new AvaloniaList<Tag>(Tags.OrderByDescending(x => x.Count));
    }

    public string Lock(string masterPassword)
    {
        string encryptJson;
        
        {
            string databaseJson = ToJson();
            string compressedJson = Compressor.Compress(databaseJson);
            encryptJson = Encryptor.EncryptString(compressedJson, masterPassword, Settings.Iterations);
        }

        string file = $"{Settings.Name}:{Settings.Iterations}:{encryptJson}:{Settings.UseTrashcan}:";

        file += Settings.ImageData ?? "none";

        return file;
    }

    public static Database Unlock(string fileContent, string masterPassword)
    {
        string[] split = fileContent.Split(':');

        string name = split[0];
        int iterations = int.Parse(split[1]);
        string encryptString = split[2];
        bool useTrashcan = bool.Parse(split[3]);
        string? imageData = split[4];
        
        if (imageData == "none")
            imageData = null;

        {
            string compressedString = Encryptor.DecryptString(encryptString, masterPassword, iterations);
            fileContent = Compressor.Decompress(compressedString);
        }

        Database database = FromJson(fileContent);

        DatabaseSettings settings = new()
        {
            Iterations = iterations,
            UseTrashcan = useTrashcan,
            Name = name,
            ImageData = imageData,
        };

        database.Settings = settings;
        
        database.UpdateInfo();
        
        return database;
    }

    public void Save(string path, string masterPassword)
    {
        string fileContent = Lock(masterPassword);

        File.WriteAllText(path, fileContent);
    }

    public static Database Load(string path, string masterPassword)
    {
        string file = File.ReadAllText(path);

        return Unlock(file, masterPassword);
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this, GenerationContexts.DatabaseGenerationContext.Default.Database);
    }

    public static Database FromJson(string json)
    {
        return JsonSerializer.Deserialize(json, GenerationContexts.DatabaseGenerationContext.Default.Database) ?? throw new NullReferenceException();
    }
}