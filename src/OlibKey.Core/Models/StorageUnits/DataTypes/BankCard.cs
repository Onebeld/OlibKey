using System.Text.Json.Serialization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using OlibKey.Core.Enums;
using OlibKey.Core.Extensions;

namespace OlibKey.Core.Models.StorageUnits.DataTypes;

public class BankCard : Data
{
	private string? _typeBankCard;
	private string? _cardNumber;
	private string? _dateCard;
	private string? _securityCode;

	public string? TypeBankCard
	{
		get => _typeBankCard;
		set => RaiseAndSetNullIfEmpty(ref _typeBankCard, value);
	}

	public string? CardNumber
	{
		get => _cardNumber;
		set => RaiseAndSetNullIfEmpty(ref _cardNumber, value);
	}

	public string? DateCard
	{
		get => _dateCard;
		set => RaiseAndSetNullIfEmpty(ref _dateCard, value);
	}

	public string? SecurityCode
	{
		get => _securityCode;
		set => RaiseAndSetNullIfEmpty(ref _securityCode, value);
	}

	public override async Task<IImage?> GetIcon()
	{
		if (IsSelectedImage)
		{
			Bitmap? bitmap = StringImageToBitmap(Image);

			if (bitmap is not null)
				return bitmap;
		}
		
		return await Task.FromResult<IImage>((DrawingImage)Application.Current!.FindResource("CardIcon")!);
	}

	public override bool MatchesSearchCriteria(string text)
	{
		if (CardNumber.IsDesiredString(text))
			return true;

		return base.MatchesSearchCriteria(text);
	}

	public override bool MatchesDataType(DataType dataType) => dataType is DataType.BankCard;

	[JsonIgnore]
	public override string? Information
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(CardNumber))
				return CardNumber;

			return null;
		}
	}
}