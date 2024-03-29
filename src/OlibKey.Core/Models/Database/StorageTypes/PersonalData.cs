using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using OlibKey.Core.Extensions;

namespace OlibKey.Core.Models.Database.StorageTypes;

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
        set => RaiseAndSet(ref _fullname, value);
    }

    public string? Number
    {
        get => _number;
        set => RaiseAndSet(ref _number, value);
    }

    public string? PlaceOfIssue
    {
        get => _placeOfIssue;
        set => RaiseAndSet(ref _placeOfIssue, value);
    }

    public string? SocialSecurityNumber
    {
        get => _socialSecurityNumber;
        set => RaiseAndSet(ref _socialSecurityNumber, value);
    }

    public string? Tin
    {
        get => _tin;
        set => RaiseAndSet(ref _tin, value);
    }

    public string? Email
    {
        get => _email;
        set => RaiseAndSet(ref _email, value);
    }

    public string? Telephone
    {
        get => _telephone;
        set => RaiseAndSet(ref _telephone, value);
    }

    public string? Company
    {
        get => _company;
        set => RaiseAndSet(ref _company, value);
    }

    public string? Postcode
    {
        get => _postcode;
        set => RaiseAndSet(ref _postcode, value);
    }

    public string? Country
    {
        get => _country;
        set => RaiseAndSet(ref _country, value);
    }

    public string? Region
    {
        get => _region;
        set => RaiseAndSet(ref _region, value);
    }

    public string? City
    {
        get => _city;
        set => RaiseAndSet(ref _city, value);
    }

    public string? Address
    {
        get => _address;
        set => RaiseAndSet(ref _address, value);
    }

    public override async Task<IImage> GetIcon()
    {
        return (DrawingImage)Application.Current!.FindResource("PersonalDataIcon")!;
    }
    
    public override bool IsDesired(string text)
    {
        if (Fullname.IsDesiredString(text))
            return true;
        
        return base.IsDesired(text);
    }

    public override string Information
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(Fullname))
                return Fullname;

            return OlibKeyApp.GetResource<string>("NoData");
        }
    }
}