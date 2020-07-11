using Avalonia.Controls;
using Avalonia.Media;

namespace OlibKey.Core
{
    public static class ItemControls
	{
        public static void ColorProgressBar(ProgressBar b)
        {
			if (b.Value < 100) b.Foreground = new SolidColorBrush(Color.FromRgb(196, 20, 3));
			else b.Foreground = b.Value < 200 ? new SolidColorBrush(Color.FromRgb(222, 222, 64)) : new SolidColorBrush(Color.FromRgb(27, 199, 11));
		}
    }
}
