using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using OlibKey.Core.Enums;

namespace OlibKey.Core.Models.Database.StorageTypes;

public class Note : Data
{
	public override async Task<IImage?> GetIcon() => await Task.FromResult<IImage>((DrawingImage)Application.Current!.FindResource("NoteIcon")!);

	public override bool MatchesDataType(DataType dataType) => dataType is DataType.Note;
}