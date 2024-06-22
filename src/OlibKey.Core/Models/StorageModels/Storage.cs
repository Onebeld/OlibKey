using System.Collections.Specialized;
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

	[JsonIgnore] public int LoginCount => Data.OfType<Login>().Count();

	[JsonIgnore] public int BankCardCount => Data.OfType<BankCard>().Count();

	[JsonIgnore] public int PersonalDataCount => Data.OfType<PersonalData>().Count();

	[JsonIgnore] public int NotesCount => Data.OfType<Note>().Count();

	[JsonIgnore] public int FavoritesCount => Data.Count(data => data.IsFavorite);


	[JsonIgnore]
	public AvaloniaList<Tag> Tags
	{
		get => _tags;
		private set => RaiseAndSet(ref _tags, value);
	}

	private void DataOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		UpdateCounts();
	}

	public void Save(string path, string masterPassword)
	{
		OlibFileLoader.Save(this, path, masterPassword);
	}

	public static Storage Load(string path, string masterPassword)
	{
		Storage storage = OlibFileLoader.Load(path, masterPassword);
		
		storage.UpdateInfo();

		return storage;
	}

	private void UpdateInfo()
	{
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

	private void GetTags()
	{
		IEnumerable<IGrouping<string, string>> tags = Data.SelectMany(x => x.Tags).GroupBy(tag => tag);

		foreach (IGrouping<string, string> grouping in tags)
			Tags.Add(new Tag(grouping.Key, grouping.Count()));

		Tags = new AvaloniaList<Tag>(Tags.OrderByDescending(x => x.Count));
	}
}