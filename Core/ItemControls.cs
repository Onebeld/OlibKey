using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OlibKey.Core
{
    public static class ItemControls
    {
        public static void ColorProgressBar(ProgressBar b)
        {
            if (b.Value < 100) b.Foreground = new SolidColorBrush(Color.FromRgb(196, 20, 3));
            else if (b.Value < 200) b.Foreground = new SolidColorBrush(Color.FromRgb(222, 222, 64));
            else b.Foreground = new SolidColorBrush(Color.FromRgb(27, 199, 11));
        }

        public static childItem FindVisualChild<childItem>(DependencyObject obj)
            where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}
