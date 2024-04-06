using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Avalonia.Collections;
using OlibKey.Core.Helpers;
using OlibKey.Core.Models.StorageModels.StorageTypes;
using OlibKey.Core.Settings;
using PleasantUI;

namespace OlibKey.Core.Models.StorageModels;

public class Storage : ViewModelBase
{
	private AvaloniaList<Data> _data = [];
	private StorageSettings _settings = null!;

	private AvaloniaList<Tag> _tags = [];
	private Trashcan _trashcan = new();

	public AvaloniaList<Data> Data
	{
		get => _data;
		set
		{
			RaiseAndSet(ref _data, value);

			value.CollectionChanged += DataOnCollectionChanged;
			UpdateInfo();

			foreach (Data data in value) 
				data.PropertyChanged += DataOnPropertyChanged;
		}
	}

	public Trashcan Trashcan
	{
		get => _trashcan;
		set => RaiseAndSet(ref _trashcan, value);
	}

	public StorageSettings Settings
	{
		get => _settings;
		set => RaiseAndSet(ref _settings, value);
	}

	[JsonIgnore] public int LoginCount { get; private set; }

	[JsonIgnore] public int BankCardCount { get; private set; }

	[JsonIgnore] public int PersonalDataCount { get; private set; }

	[JsonIgnore] public int NotesCount { get; private set; }

	[JsonIgnore] public int FavoritesCount { get; private set; }


	[JsonIgnore]
	public AvaloniaList<Tag> Tags
	{
		get => _tags;
		private set => RaiseAndSet(ref _tags, value);
	}

	private void DataOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if (e.NewItems is not null)
			ChangeCountValues(e.NewItems, 1);

		if (e.OldItems is not null)
			ChangeCountValues(e.OldItems, -1, false);

		UpdateCounts();
	}

	private void ChangeCountValues(IList list, int value, bool adding = true)
	{
		foreach (object item in list)
		{
			if (item is not Data data) continue;
			
			if (adding)
				data.PropertyChanged += DataOnPropertyChanged;
			else 
				data.PropertyChanged -= DataOnPropertyChanged;

			if (data.GetType() == typeof(Note))
				NotesCount += value;
			else if (data.GetType() == typeof(Login))
				LoginCount += value;
			else if (data.GetType() == typeof(BankCard))
				BankCardCount += value;
			else if (data.GetType() == typeof(PersonalData))
				PersonalDataCount += value;

			if (data.IsFavorite) FavoritesCount += value;
		}
	}

	private void DataOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (sender is not Data data) return;

		if (e.PropertyName == nameof(StorageModels.Data.IsFavorite))
		{
			if (data.IsFavorite) FavoritesCount++;
			else FavoritesCount--;

			RaisePropertyChanged(nameof(FavoritesCount));
		}
	}

	public string Lock(string masterPassword)
	{
		string encryptJson;

		{
			string storageJson = ToJson();
			string compressedJson = Compressor.Compress(storageJson);
			encryptJson = Encryptor.EncryptString(compressedJson, masterPassword, Settings.Iterations);
		}

		string file = $"{Settings.Name}:{Settings.Iterations}:{encryptJson}:{Settings.UseTrashcan}:";

		file += Settings.ImageData ?? "none";

		return file;
	}

	public static Storage Unlock(string fileContent, string masterPassword)
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

		Storage storage = FromJson(fileContent);

		StorageSettings settings = new()
		{
			Iterations = iterations,
			UseTrashcan = useTrashcan,
			Name = name,
			ImageData = imageData,
		};

		storage.Settings = settings;

		storage.UpdateInfo();

		return storage;
	}

	public void Save(string path, string masterPassword)
	{
		string fileContent = Lock(masterPassword);

		File.WriteAllText(path, fileContent);
	}

	public static Storage Load(string path, string masterPassword)
	{
		string file = File.ReadAllText(path);

		return Unlock(file, masterPassword);
	}

	public string ToJson()
	{
		return JsonSerializer.Serialize(this, GenerationContexts.StorageGenerationContext.Default.Storage);
	}

	public static Storage FromJson(string json)
	{
		return JsonSerializer.Deserialize(json, GenerationContexts.StorageGenerationContext.Default.Storage) ??
		       throw new NullReferenceException();
	}

	private void UpdateInfo()
	{
		CalculateCountOfTypes();

		Tags.Clear();
		GetTags();

		UpdateCounts();
	}

	private void UpdateCounts()
	{
		RaisePropertyChanged(nameof(LoginCount));
		RaisePropertyChanged(nameof(BankCardCount));
		RaisePropertyChanged(nameof(PersonalDataCount));
		RaisePropertyChanged(nameof(NotesCount));
		RaisePropertyChanged(nameof(FavoritesCount));
	}

	private void CalculateCountOfTypes()
	{
		LoginCount = 0;
		BankCardCount = 0;
		PersonalDataCount = 0;
		NotesCount = 0;
		FavoritesCount = 0;

		foreach (Data data in Data)
		{
			if (data.GetType() == typeof(Note))
				NotesCount++;
			else if (data.GetType() == typeof(Login))
				LoginCount++;
			else if (data.GetType() == typeof(BankCard))
				BankCardCount++;
			else if (data.GetType() == typeof(PersonalData))
				PersonalDataCount++;

			if (data.IsFavorite)
				FavoritesCount++;
		}
	}

	private void GetTags()
	{
		IEnumerable<IGrouping<string, string>> tags = Data.SelectMany(x => x.Tags).GroupBy(tag => tag);

		foreach (IGrouping<string, string> grouping in tags)
			Tags.Add(new Tag(grouping.Key, grouping.Count()));

		Tags = new AvaloniaList<Tag>(Tags.OrderByDescending(x => x.Count));
	}
}