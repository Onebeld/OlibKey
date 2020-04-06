using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Effects;

namespace OlibKey.Core
{
    internal class InvertEffect : ShaderEffect
    {
        private const string V =
            @"AAP///7/IQBDVEFCHAAAAE8AAAAAA///AQAAABwAAAAAAQAASAAAADAAAAADAAAAAQACADgAAAAAAAAAaW5wdXQAq6sEAAwAAQABAAEAAAAAAAAAcHNfM18wAE1pY3Jvc29mdCAoUikgSExTTCBTaGFkZXIgQ29tcGlsZXIgOS4yNy45NTIuMzAyMgAfAAACBQAAgAAAA5AfAAACAAAAkAAID6BCAAADAAAPgAAA5JAACOSgAQAAAgAID4AAAP+A//8AAA==";

        private static readonly PixelShader _shader;

        static InvertEffect()
        {
            _shader = new PixelShader();
            _shader.SetStreamSource(new MemoryStream(Convert.FromBase64String(V)));
        }

        public InvertEffect()
        {
            PixelShader = _shader;
            UpdateShaderValue(InputProperty);
        }

        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("Input", typeof(InvertEffect), 0);
    }
}
