using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace OlibPasswordManager.Properties.Core
{
    public class ItemControls
    {
        public static void ColorProgressBar(ProgressBar b)
        {
            if (b.Value < 100) b.Foreground = new SolidColorBrush(Color.FromRgb(196, 20, 3));
            else if (b.Value < 200) b.Foreground = new SolidColorBrush(Color.FromRgb(222, 222, 64));
            else b.Foreground = new SolidColorBrush(Color.FromRgb(27, 199, 11));
        }
    }
}
