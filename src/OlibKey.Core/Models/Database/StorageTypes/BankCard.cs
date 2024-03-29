using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using OlibKey.Core.Extensions;

namespace OlibKey.Core.Models.Database.StorageTypes;

public class BankCard : Data
{
    private string? _typeBankCard;
    private string? _cardNumber;
    private string? _dateCard;
    private string? _securityCode;

    public string? TypeBankCard
    {
        get => _typeBankCard;
        set => RaiseAndSet(ref _typeBankCard, value);
    }

    public string? CardNumber
    {
        get => _cardNumber;
        set => RaiseAndSet(ref _cardNumber, value);
    }

    public string? DateCard
    {
        get => _dateCard;
        set => RaiseAndSet(ref _dateCard, value);
    }

    public string? SecurityCode
    {
        get => _securityCode;
        set => RaiseAndSet(ref _securityCode, value);
    }

    public override async Task<IImage> GetIcon()
    {
        return (DrawingImage)Application.Current!.FindResource("CardIcon")!;
    }

    public override bool IsDesired(string text)
    {
        if (CardNumber.IsDesiredString(text))
            return true;
        
        return base.IsDesired(text);
    }

    public override string Information
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(CardNumber))
                return CardNumber;
        
            return OlibKeyApp.GetResource<string>("NoData");
        }
    }
}