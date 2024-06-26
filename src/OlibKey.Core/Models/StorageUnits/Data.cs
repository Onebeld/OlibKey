using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Avalonia.Collections;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using OlibKey.Core.Enums;
using OlibKey.Core.Extensions;
using OlibKey.Core.Helpers;
using OlibKey.Core.Models.StorageUnits.DataTypes;
using PleasantUI;

namespace OlibKey.Core.Models.StorageUnits;

[JsonDerivedType(typeof(Login), typeDiscriminator: "login")]
[JsonDerivedType(typeof(BankCard), typeDiscriminator: "bankcard")]
[JsonDerivedType(typeof(PersonalData), typeDiscriminator: "personalData")]
[JsonDerivedType(typeof(Note), typeDiscriminator: "note")]
public class Data : ViewModelBase, ICloneable
{
	private string? _image;
	
	private DateTime? _timeCreate;
	private DateTime? _timeChanged;
	
	private string? _name;
	private string? _deleteDate;
	private uint? _color;
	private string? _note;
	private bool _isSelectedImage;

	private bool _isFavorite;

	private AvaloniaList<string> _tags = [];
	private AvaloniaList<Section> _sections = [];

	public string? Name
	{
		get => _name;
		set => RaiseAndSetNullIfEmpty(ref _name, value);
	}

	public DateTime? TimeCreate
	{
		get => _timeCreate;
		set => RaiseAndSet(ref _timeCreate, value);
	}

	public DateTime? TimeChanged
	{
		get => _timeChanged;
		set => RaiseAndSet(ref _timeChanged, value);
	}

	public string? DeleteDate
	{
		get => _deleteDate;
		set => RaiseAndSet(ref _deleteDate, value);
	}

	public uint? Color
	{
		get => _color;
		set => RaiseAndSet(ref _color, value);
	}

	public string? Note
	{
		get => _note;
		set => RaiseAndSetNullIfEmpty(ref _note, value);
	}

	public bool IsFavorite
	{
		get => _isFavorite;
		set
		{
			RaiseAndSet(ref _isFavorite, value);
			
			OlibKeyApp.ViewModel.Session.Storage?.UpdateFavoritesCount();
			OlibKeyApp.ViewModel.IsDirty = true;
		}
	}

	public bool IsSelectedImage
	{
		get => _isSelectedImage;
		set => RaiseAndSet(ref _isSelectedImage, value);
	}

	public string? Image
	{
		get => _image;
		set => RaiseAndSet(ref _image, value);
	}

	public AvaloniaList<string> Tags
	{
		get => _tags;
		set => RaiseAndSet(ref _tags, value);
	}

	public AvaloniaList<Section> Sections
	{
		get => _sections;
		set => RaiseAndSet(ref _sections, value);
	}

	#region JsonIgnored

	[JsonIgnore] public Task<IImage?> Icon => GetIcon();
	
	[JsonIgnore] public virtual string? Information => null;

	#endregion

	public virtual Task<IImage?> GetIcon() => Task.FromResult<IImage?>(null);
	
	public virtual bool MatchesSearchCriteria(string text)
	{
		return Name.IsDesiredString(text);
	}

	public virtual bool MatchesDataType(DataType dataType) => dataType is DataType.All;

	public T ConvertData<T>() where T : Data, new()
	{
		return new T
		{
			Name = Name,
			TimeCreate = TimeCreate,
			TimeChanged = TimeChanged,
			DeleteDate = DeleteDate,
			Color = Color,
			Note = Note,
			IsFavorite = IsFavorite,
			Tags = new AvaloniaList<string>(Tags),
			Sections = new AvaloniaList<Section>(Sections)
		};
	}

	protected bool RaiseAndSetNullIfEmpty(ref string? field, string? value,
		[CallerMemberName] string? propertyName = null)
	{
		if (string.IsNullOrWhiteSpace(value))
			value = null;

		return RaiseAndSet(ref field, value, propertyName);
	}

	protected Bitmap? StringImageToBitmap(string? image)
	{
		try
		{
			using MemoryStream ms = new(Convert.FromBase64String(image ?? throw new NullReferenceException()));
			return new Bitmap(ms);
		}
		catch (Exception)
		{
			return null;
		}
	}

	public void UpdateIcon()
	{
		RaisePropertyChanged(nameof(Icon));
	}
	
	public void ChangeImageFromPath(string path)
	{
		Bitmap bitmap = new Bitmap(path).ChangeSize(32, 32);

		Image = bitmap.ToBase64String();
	}

	public object Clone()
	{
		Data data = (Data)MemberwiseClone();

		data.Tags = new AvaloniaList<string>(Tags);
		data.Sections = new AvaloniaList<Section>(Sections);

		return data;
	}
}