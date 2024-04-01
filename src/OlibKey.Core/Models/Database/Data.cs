using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Avalonia.Collections;
using Avalonia.Media;
using OlibKey.Core.Enums;
using OlibKey.Core.Extensions;
using OlibKey.Core.Models.Database.StorageTypes;
using PleasantUI;

namespace OlibKey.Core.Models.Database;

[JsonDerivedType(typeof(Login), typeDiscriminator: "login")]
[JsonDerivedType(typeof(BankCard), typeDiscriminator: "bankcard")]
[JsonDerivedType(typeof(PersonalData), typeDiscriminator: "personalData")]
[JsonDerivedType(typeof(Note), typeDiscriminator: "note")]
public class Data : ViewModelBase, ICloneable
{
    private string? _name;
    private string? _timeCreate;
    private string? _timeChanged;
    private string? _deleteDate;
    private uint? _color;
    private string? _note;
    
    private bool _isFavorite;

    private AvaloniaList<string> _tags = [];
    
    public string? Name
    {
        get => _name;
        set => RaiseAndSetNullIfEmpty(ref _name, value);
    }

    public string? TimeCreate
    {
        get => _timeCreate;
        set => RaiseAndSet(ref _timeCreate, value);
    }

    public string? TimeChanged
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
        set
        {
            if (value == string.Empty)
                value = null;
            
            RaiseAndSet(ref _note, value);
        }
    }

    public bool IsFavorite
    {
        get => _isFavorite;
        set => RaiseAndSet(ref _isFavorite, value);
    }

    public AvaloniaList<string> Tags
    {
        get => _tags;
        set => RaiseAndSet(ref _tags, value);
    }
    
    #region JsonIgnored

    [JsonIgnore]
    public bool IsDeleted;
    
    [JsonIgnore]
    public bool IsIconChange;
    
    [JsonIgnore]
    public Task<IImage?> Icon => GetIcon();

    #endregion

    public virtual Task<IImage?> GetIcon() => Task.FromResult<IImage?>(null);

    [JsonIgnore]
    public virtual string? Information => null;

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
            Tags = new AvaloniaList<string>(Tags)
        };
    }

    protected bool RaiseAndSetNullIfEmpty(ref string? field, string? value, [CallerMemberName] string? propertyName = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            value = null;
        
        return RaiseAndSet(ref field, value, propertyName);
    }

    public void UpdateIcon()
    {
        RaisePropertyChanged(nameof(Icon));
    }

    public object Clone()
    {
        Data data = (Data)MemberwiseClone();

        data.Tags = new AvaloniaList<string>(Tags);

        return data;
    }
}