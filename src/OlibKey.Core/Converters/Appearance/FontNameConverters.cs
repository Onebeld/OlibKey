using Avalonia.Data.Converters;
using Avalonia.Media;

namespace OlibKey.Core.Converters.Appearance;

public static class FontNameConverters
{
	public static IValueConverter NameToFontFamily =>
		new FuncValueConverter<string, FontFamily>(fontName =>
		{
			try
			{
				if (fontName is not null)
					return FontFamily.Parse(fontName);
			}
			catch
			{
				return FontFamily.Default;
			}

			return FontFamily.Default;
		});
}