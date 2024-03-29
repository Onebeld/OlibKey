using System.Text.Json.Serialization;
using Avalonia.Collections;
using Avalonia.Media;
using OlibKey.Core.Extensions;
using PleasantUI;

namespace OlibKey.Core.Models.Database;

public class Data : ViewModelBase, ICloneable
{
    private string? _name;
    private string? _timeCreate;
    private string? _timeChanged;
    private string? _deleteDate;
    private uint _color;
    private bool _useColor;
    private string? _note;
    
    private bool _isFavorite;

    private AvaloniaList<string> _tags = [];
    
    public string? Name
    {
        get => _name;
        set => RaiseAndSet(ref _name, value);
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

    public uint Color
    {
        get => _color;
        set => RaiseAndSet(ref _color, value);
    }

    public bool UseColor
    {
        get => _useColor;
        set => RaiseAndSet(ref _useColor, value);
    }
    
    public string? Note
    {
        get => _note;
        set => RaiseAndSet(ref _note, value);
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
    public Task<IImage> Icon => GetIcon();

    #endregion

    public virtual Task<IImage> GetIcon() => throw new NotImplementedException();

    public virtual string Information => string.Empty;

    public virtual bool IsDesired(string text)
    {
        return Name.IsDesiredString(text);
    }

    public object Clone()
    {
        Data data = (Data)MemberwiseClone();

        data.Tags = new AvaloniaList<string>(Tags);

        return data;
    }
}