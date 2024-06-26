using System.Collections.Specialized;
using System.Reflection;
using System.Security.Cryptography;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using OlibKey.Core.Enums;
using OlibKey.Core.Helpers;
using OlibKey.Core.Models;
using OlibKey.Core.Models.StorageUnits;
using OlibKey.Core.Settings;
using OlibKey.Core.StaticMembers;
using OlibKey.Core.Structures;
using OlibKey.Core.Views.ViewerPages;
using OlibKey.Core.Windows;
using PleasantUI;
using PleasantUI.Controls;
using PleasantUI.Extensions;
using PleasantUI.Reactive;

namespace OlibKey.Core.ViewModels;

public class ApplicationViewModel : ViewModelBase
{
	private Session _session;

	private Data? _selectedData;
	private DataType _selectedDataType = DataType.All;

	private bool _sortingByAlpha;
	private bool _sortingByDate;
	private bool _showFavorites;

	private bool _isDirty;

	private bool _isLoading;

	private string _searchText = string.Empty;

	private AvaloniaList<Tag> _selectedTags = [];
	private AvaloniaList<Data> _foundedDataList = [];

	private Type? _viewerType;

	private string _masterPassword = string.Empty;

	#region EventHandlers

	public event EventHandler? StorageCreated;
	public event EventHandler? StorageOpened;
	public event EventHandler? StorageBlocking;
	public event EventHandler? StorageBlocked;
	public event EventHandler? StorageUnlocked;

	#endregion

	#region Properties

	public IManagedNotificationManager? NotificationManager { get; set; }

	public Session Session
	{
		get => _session;
		set => RaiseAndSet(ref _session, value);
	}

	public Data? SelectedData
	{
		get => _selectedData;
		set
		{
			RaiseAndSet(ref _selectedData, value);

			ShowData(value);
		}
	}

	public bool SortingByAlpha
	{
		get => _sortingByAlpha;
		set
		{
			RaiseAndSet(ref _sortingByAlpha, value);

			DoSearch();
		}
	}

	public bool SortingByDate
	{
		get => _sortingByDate;
		set
		{
			RaiseAndSet(ref _sortingByDate, value);

			DoSearch();
		}
	}

	public bool ShowFavorites
	{
		get => _showFavorites;
		set
		{
			RaiseAndSet(ref _showFavorites, value);

			DoSearch();
		}
	}

	public bool IsDirty
	{
		get => _isDirty;
		set => RaiseAndSet(ref _isDirty, value);
	}

	public bool IsLoading
	{
		get => _isLoading;
		set => RaiseAndSet(ref _isLoading, value);
	}

	public AvaloniaList<Tag> SelectedTags
	{
		get => _selectedTags;
		set
		{
			RaiseAndSet(ref _selectedTags, value);

			value.CollectionChanged += SelectedTagsOnCollectionChanged;
		}
	}

	public AvaloniaList<Data> FoundedDataList
	{
		get => _foundedDataList;
		set => RaiseAndSet(ref _foundedDataList, value);
	}

	public Control? ViewerContent
	{
		get
		{
			if (_viewerType is null)
				return null;
			
			if (_viewerType == typeof(OlibKeyPage))
				return new OlibKeyPage();

			if (_viewerType == typeof(DataPage))
				return new DataPage(SelectedData);

			throw new NotSupportedException();
		}
	}

	public string SearchText
	{
		get => _searchText;
		set => RaiseAndSet(ref _searchText, value);
	}

	public string MasterPassword
	{
		get => _masterPassword;
		set => RaiseAndSet(ref _masterPassword, value);
	}

	public DataType SelectedDataType
	{
		get => _selectedDataType;
		set
		{
			RaiseAndSet(ref _selectedDataType, value);

			DoSearch();
		}
	}

	#endregion

	public ApplicationViewModel()
	{
		this.WhenAnyValue(x => x.SearchText)
			.Skip(1)
			.Subscribe(_ => DoSearch());

		SelectedTags.CollectionChanged += SelectedTagsOnCollectionChanged;

		Session = new Session();
	}

	public async void CreateStorage()
	{
		if (OlibKeyApp.Main.ModalWindows.Any(modalWindow => modalWindow is CreateStorageWindow))
			return;

		CreateStorageWindow window = new();
		StorageInfo? result = await window.Show<StorageInfo?>(OlibKeyApp.Main);

		if (result is null) return;

		if (Session.Storage is not null) 
			await LockStorage();

		Session.CreateStorage(result.Value);

		IsDirty = true;

		SetViewerType(typeof(OlibKeyPage));

		DoSearch();

		StorageCreated?.Invoke(this, EventArgs.Empty);

		OlibKeyApp.ShowNotification("Successful", "StorageCreated", NotificationType.Success);
	}

	public async void OpenStorage()
	{
		string? path = await StorageProviderUtils.SelectFile(pickerFileTypes: FileTypes.Olib);

		if (path is null) return;

		if (Session.Storage is not null) 
			await LockStorage();

		Session.OpenStorage(path);

		StorageOpened?.Invoke(this, EventArgs.Empty);
	}

	public async void UnlockStorage()
	{
		try
		{
			IsLoading = true;

			await Task.Run(() =>
			{
				Session.UnlockStorage(MasterPassword);
			});

			MasterPassword = string.Empty;

			DoSearch();

			SetViewerType(typeof(OlibKeyPage));

			StorageUnlocked?.Invoke(this, EventArgs.Empty);
		}
		catch (CryptographicException)
		{
			OlibKeyApp.ShowNotification("Error", "InvalidMasterPassword", NotificationType.Error);
		}
		catch (FileNotFoundException)
		{
			OlibKeyApp.ShowNotification("Error", "StorageFileNotFound", NotificationType.Error);
		}
		catch (Exception)
		{
			OlibKeyApp.ShowNotification("Error", "UnknownErrorOccurred", NotificationType.Error);
		}
		
		IsLoading = false;
	}

	public void AddData()
	{
		SelectedData = null;

		SetViewerType(typeof(DataPage));
	}

	public void ShowData(Data? data)
	{
		SetViewerType(null);
		
		if (data is null)
		{
			SetViewerType(typeof(OlibKeyPage));
			return;
		}

		SetViewerType(typeof(DataPage));
	}

	public async Task LockStorage()
	{
		StorageBlocking?.Invoke(this, EventArgs.Empty);

		IsLoading = true;

		await Task.Run(() =>
		{
			Session.LockStorage();
		});
			
		IsLoading = false;

		SetViewerType(typeof(OlibKeyPage));

		StorageBlocked?.Invoke(this, EventArgs.Empty);
	}

	public async void OpenStorageSettings()
	{
		if (Session.Storage is null)
			return;

		Session.RestartLockerTimer();

		StorageSettingsWindow window = new(Session.Storage.Settings);

		StorageSettings? newSettings = await window.Show<StorageSettings>(OlibKeyApp.Main);

		if (newSettings is null)
			return;

		Session.Storage.Settings = newSettings;

		IsDirty = true;
	}

	public void OpenTrashcanWindow()
	{
		Session.RestartLockerTimer();

		// TODO: Open trashcan
	}

	public void OpenPasswordCheckerWindow()
	{
		Session.RestartLockerTimer();

		// TODO: Open password checker
	}

	public void ActivateLockerTimer()
	{
		Session.ActivateLockerTimer(OlibKeySettings.Instance.LockoutTime, TickLockerTimer);

		// TODO: Activate locker timer
	}

	private void TickLockerTimer(object? sender, EventArgs e)
	{
		foreach (PleasantModalWindow modalWindow in OlibKeyApp.Main.ModalWindows)
			modalWindow.Close();

		LockStorage();
	}

	public void DoSearch()
	{
		if (Session.Storage is null) return;

		Session.RestartLockerTimer();

		List<Data> results = new(Session.Storage.Data);

		if (SelectedDataType is not DataType.All)
			results = results.FindAll(data => data.MatchesDataType(SelectedDataType));

		if (!string.IsNullOrWhiteSpace(SearchText))
			results = results.FindAll(data => data.MatchesSearchCriteria(SearchText));

		if (SelectedTags.Count > 0)
		{
			List<Data> resultsFromTags = [];

			foreach (Tag tag in SelectedTags)
				resultsFromTags.AddRange(results.FindAll(data => data.Tags.Any(x1 => x1 == tag.Name)));

			results = resultsFromTags.Distinct().ToList();
		}

		if (SortingByAlpha)
			results = results.OrderBy(x => x.Name).ToList();

		if (SortingByDate)
			results = results.OrderByDescending(x => x.TimeChanged ?? x.TimeCreate).ToList();

		if (ShowFavorites)
			results = results.FindAll(data => data.IsFavorite);

		FoundedDataList = new AvaloniaList<Data>(results);
	}

	public async void Save()
	{
		IsLoading = true;

		await Task.Run(() =>
		{
			Session.SaveStorage();
		});

		IsLoading = false;

		IsDirty = false;

		OlibKeyApp.ShowNotification("Successful", "StorageSaved", NotificationType.Success);
	}

	private void SelectedTagsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => DoSearch();

	public async void CheckUpdate(bool showMessage = false)
	{
		Update? update;
		
		try
		{
			update = await Update.Get();
		}
		catch
		{
			return;
		}
		
		if (update is null) return;

		if (update.Version > Assembly.GetExecutingAssembly().GetName().Version)
		{
			UpdateWindow updateWindow = new(update);
			updateWindow.Show(OlibKeyApp.Main);
		}
		else
		{
			if (showMessage)
				OlibKeyApp.ShowNotification("Information", "NoUpdateRequired", NotificationType.Information);
		}
	}

	public void OpenOlibKey()
	{
		if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime)
			return;

		OlibKeyApp.OpenMainWindowAction?.Invoke();
	}

	public void CloseOlibKey()
	{
		if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
			return;

		desktop.TryShutdown();
	}

	public void SetViewerType(Type? type)
	{
		_viewerType = type;
		RaisePropertyChanged(nameof(ViewerContent));
	} 
}