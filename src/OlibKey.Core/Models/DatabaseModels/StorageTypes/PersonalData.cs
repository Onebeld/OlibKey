using System.Text.Json.Serialization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using OlibKey.Core.Enums;
using OlibKey.Core.Extensions;

namespace OlibKey.Core.Models.DatabaseModels.StorageTypes;

public class PersonalData : Data
{
    private string? _fullname;
    private string? _number;
    private string? _placeOfIssue;
    private string? _socialSecurityNumber;
    private string? _tin;
    private string? _email;
    private string? _telephone;
    private string? _company;
    private string? _postcode;
    private string? _country;
    private string? _region;
    private string? _city;
    private string? _address;

    public string? Fullname
    {
        get => _fullname;
        set => RaiseAndSetNullIfEmpty(ref _fullname, value);
    }

    public string? Number
    {
        get => _number;
        set => RaiseAndSetNullIfEmpty(ref _number, value);
    }

    public string? PlaceOfIssue
    {
        get => _placeOfIssue;
        set => RaiseAndSetNullIfEmpty(ref _placeOfIssue, value);
    }

    public string? SocialSecurityNumber
    {
        get => _socialSecurityNumber;
        set => RaiseAndSetNullIfEmpty(ref _socialSecurityNumber, value);
    }

    public string? Tin
    {
        get => _tin;
        set => RaiseAndSetNullIfEmpty(ref _tin, value);
    }

    public string? Email
    {
        get => _email;
        set => RaiseAndSetNullIfEmpty(ref _email, value);
    }

    public string? Telephone
    {
        get => _telephone;
        set => RaiseAndSetNullIfEmpty(ref _telephone, value);
    }

    public string? Company
    {
        get => _company;
        set => RaiseAndSetNullIfEmpty(ref _company, value);
    }

    public string? Postcode
    {
        get => _postcode;
        set => RaiseAndSet(ref _postcode, value);
    }

    public string? Country
    {
        get => _country;
        set => RaiseAndSetNullIfEmpty(ref _country, value);
    }

    public string? Region
    {
        get => _region;
        set => RaiseAndSetNullIfEmpty(ref _region, value);
    }

    public string? City
    {
        get => _city;
        set => RaiseAndSetNullIfEmpty(ref _city, value);
    }

    public string? Address
    {
        get => _address;
        set => RaiseAndSetNullIfEmpty(ref _address, value);
    }

    public override async Task<IImage?> GetIcon() => await Task.FromResult<IImage>((DrawingImage)Application.Current!.FindResource("PersonalDataIcon")!);

    public override bool MatchesSearchCriteria(string text)
    {
        if (Fullname.IsDesiredString(text))
            return true;
        
        return base.MatchesSearchCriteria(text);
    }

    public override bool MatchesDataType(DataType dataType) => dataType is DataType.PersonalData;

    [JsonIgnore]
    public override string? Information
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(Fullname))
                return Fullname;

            return null;
        }
    }
}