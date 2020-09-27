using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using OlibKey.Core.Interfaces;
using OlibKey.ViewModels.Color;

namespace OlibKey
{
    public static class Extensions
    {
        private static string Invariant(double value)
        {
            return FormattableString.Invariant($"{value}");
        }

        public static IBrush ToBursh(this Color color)
        {
            return new SolidColorBrush(color);
        }

        public static Color ToColor(this IColor color)
        {
            switch (color)
            {
                case ArgbColorViewModel argb:
                    return new Color(argb.A, argb.R, argb.G, argb.B);
                default:
                    throw new NotSupportedException($"Not supported color type {color.GetType()}.");
            }
        }

        public static ArgbColorViewModel ArgbFromColor(this Color color)
        {
            return new ArgbColorViewModel()
            {
                A = color.A,
                R = color.R,
                G = color.G,
                B = color.B
            };
        }

        public static IBrush ToBrush(this IColor color)
        {
            switch (color)
            {
                case ArgbColorViewModel argb:
                    return new SolidColorBrush(ToColor(color));
                default:
                    throw new NotSupportedException($"Not supported color type {color.GetType()}.");
            }
        }

        public static void FromTextString(this string value, out double left, out double top, out double right, out double bottom)
        {
            var thickness = Thickness.Parse(value);
            left = thickness.Left;
            top = thickness.Top;
            right = thickness.Right;
            bottom = thickness.Bottom;
        }

        public static ArgbColorViewModel ArgbFromHexString(this string value)
        {
            return Color.Parse(value).ArgbFromColor();
        }

        public static void FromHexString(this string value, out byte a, out byte r, out byte g, out byte b)
        {
            var color = Color.Parse(value);
            a = color.A;
            r = color.R;
            g = color.G;
            b = color.B;
        }
        public static uint ToUint32(this ArgbColorViewModel color)
        {
            return ((uint)color.A << 24) | ((uint)color.R << 16) | ((uint)color.G << 8) | (uint)color.B;
        }

        public static string ToHexString(this ArgbColorViewModel color)
        {
            return $"#{color.ToUint32():X8}";
        }

        public static string ToHexString3(this IList<object> values)
        {
            uint argb = ((uint)255 << 24) | ((uint)(byte)values[0] << 16) | ((uint)(byte)values[1] << 8) | (uint)(byte)values[2];
            return $"#{argb:X8}";
        }

        public static string ToHexString4(this IList<object> values)
        {
            uint argb = ((uint)(byte)values[0] << 24) | ((uint)(byte)values[1] << 16) | ((uint)(byte)values[2] << 8) | (uint)(byte)values[3];
            return $"#{argb:X8}";
        }

        public static string ToHexString(this IColor color)
        {
            switch (color)
            {
                case ArgbColorViewModel argb:
                    return $"#{argb.ToUint32():X8}";
                default:
                    throw new NotSupportedException($"Not supported color type {color.GetType()}.");
            }
        }
    }
}
