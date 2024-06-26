using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using OlibKey.Core.Enums;

namespace OlibKey.Core.Models.StorageUnits.DataTypes;

public class Note : Data
{
	public override async Task<IImage?> GetIcon()
	{
		if (IsSelectedImage)
		{
			Bitmap? bitmap = StringImageToBitmap(Image);

			if (bitmap is not null)
				return bitmap;
		}
		
		return await Task.FromResult<IImage>((DrawingImage)Application.Current!.FindResource("NoteIcon")!);
	}

	public override bool MatchesDataType(DataType dataType) => dataType is DataType.Note;
}