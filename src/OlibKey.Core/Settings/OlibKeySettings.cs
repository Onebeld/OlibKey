using System.Globalization;
using System.Text.Json;
using Avalonia.Media;
using PleasantUI;
using PleasantUI.Core.Constants;

namespace OlibKey.Core.Settings;

public class OlibKeySettings : ViewModelBase
{
	private string _language = string.Empty;
	private string _fontName = string.Empty;

	private int _generationCount = 10;
	private bool _generatorAllowLowercase = true;
	private bool _generatorAllowNumber = true;
	private bool _generatorAllowOther;
	private bool _generatorAllowSpecial;
	private bool _generatorAllowUnderscore;
	private bool _generatorAllowUppercase = true;
	private string _generatorTextOther = string.Empty;

	private bool _lockStorage = true;
	private int _lockoutTime = 30;
	private bool _clearClipboard;
	private int _clearingClipboardTime = 10;
	private bool _cleanTrashcan = true;
	private int _clearingTrashcanTime = 30;
	private double _searchSimilarity = 0.5;

	#region Properties

	/// <summary>
	/// Application language
	/// </summary>
	public string Language
	{
		get => _language;
		set => RaiseAndSet(ref _language, value);
	}

	/// <summary>
	/// Current application font
	/// </summary>
	public string FontName
	{
		get => _fontName;
		set => RaiseAndSet(ref _fontName, value);
	}

	public int GenerationCount
	{
		get => _generationCount;
		set => RaiseAndSet(ref _generationCount, value);
	}

	public bool GeneratorAllowLowercase
	{
		get => _generatorAllowLowercase;
		set => RaiseAndSet(ref _generatorAllowLowercase, value);
	}

	public bool GeneratorAllowNumber
	{
		get => _generatorAllowNumber;
		set => RaiseAndSet(ref _generatorAllowNumber, value);
	}

	public bool GeneratorAllowOther
	{
		get => _generatorAllowOther;
		set => RaiseAndSet(ref _generatorAllowOther, value);
	}

	public bool GeneratorAllowSpecial
	{
		get => _generatorAllowSpecial;
		set => RaiseAndSet(ref _generatorAllowSpecial, value);
	}

	public bool GeneratorAllowUnderscore
	{
		get => _generatorAllowUnderscore;
		set => RaiseAndSet(ref _generatorAllowUnderscore, value);
	}

	public bool GeneratorAllowUppercase
	{
		get => _generatorAllowUppercase;
		set => RaiseAndSet(ref _generatorAllowUppercase, value);
	}

	public string GeneratorTextOther
	{
		get => _generatorTextOther;
		set => RaiseAndSet(ref _generatorTextOther, value);
	}

	public bool LockStorage
	{
		get => _lockStorage;
		set => RaiseAndSet(ref _lockStorage, value);
	}

	public int LockoutTime
	{
		get => _lockoutTime;
		set => RaiseAndSet(ref _lockoutTime, value);
	}

	public bool ClearClipboard
	{
		get => _clearClipboard;
		set => RaiseAndSet(ref _clearClipboard, value);
	}

	public int ClearingTime
	{
		get => _clearingClipboardTime;
		set => RaiseAndSet(ref _clearingClipboardTime, value);
	}

	public bool CleanTrashcan
	{
		get => _cleanTrashcan;
		set => RaiseAndSet(ref _cleanTrashcan, value);
	}

	public int ClearingTrashcanTime
	{
		get => _clearingTrashcanTime;
		set => RaiseAndSet(ref _clearingTrashcanTime, value);
	}

	public double SearchSimilarity
	{
		get => _searchSimilarity;
		set => RaiseAndSet(ref _searchSimilarity, value);
	}

	#endregion

	/// <summary>
	/// An instance of the <see cref="OlibKeySettings"/> class, which defines the program settings
	/// </summary>
	public static OlibKeySettings Instance { get; private set; } = null!;

	private OlibKeySettings()
	{
	}

	static OlibKeySettings()
	{
		if (!Directory.Exists(PleasantDirectories.Settings))
			Directory.CreateDirectory(PleasantDirectories.Settings);

		string olibKeySettings = Path.Combine(PleasantDirectories.Settings, "OlibKeySettings.json");

		if (File.Exists(olibKeySettings))
		{
			try
			{
				using FileStream fileStream =
					File.OpenRead(Path.Combine(PleasantDirectories.Settings, olibKeySettings));

				Instance = JsonSerializer.Deserialize(fileStream,
					           GenerationContexts.OlibKeySettingsGenerationContext.Default.OlibKeySettings) ??
				           throw new NullReferenceException();
			}
			catch
			{
				CreateInstance();
			}
		}
		else CreateInstance();
	}

	/// <summary>
	/// Creates an instance of a class <see cref="OlibKeySettings"/>
	/// </summary>
	private static void CreateInstance()
	{
		Instance = new OlibKeySettings
		{
			Language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName
		};

		Setup();
	}

	/// <summary>
	/// Sets the initial settings for a <see cref="OlibKeySettings"/> class instance
	/// </summary>
	private static void Setup()
	{
		Instance.FontName = FontManager.Current.DefaultFontFamily.Name;
	}

	public static void Save()
	{
		using FileStream fileStream = File.Create(Path.Combine(PleasantDirectories.Settings, "OlibKeySettings.json"));
		JsonSerializer.Serialize(fileStream, Instance,
			GenerationContexts.OlibKeySettingsGenerationContext.Default.OlibKeySettings);
	}

	public static void Reset()
	{
	}
}