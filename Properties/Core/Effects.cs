using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Effects;

namespace OlibPasswordManager.Properties.Core
{
    internal class InvertEffect : ShaderEffect
    {
        private const string V =
            @"AAP///7/IQBDVEFCHAAAAE8AAAAAA///AQAAABwAAAAAAQAASAAAADAAAAADAAAAAQACADgAAAAAAAAAaW5wdXQAq6sEAAwAAQABAAEAAAAAAAAAcHNfM18wAE1pY3Jvc29mdCAoUikgSExTTCBTaGFkZXIgQ29tcGlsZXIgOS4yNy45NTIuMzAyMgAfAAACBQAAgAAAA5AfAAACAAAAkAAID6BCAAADAAAPgAAA5JAACOSgAQAAAgAID4AAAP+A//8AAA==";

        private static readonly PixelShader Shader;

        static InvertEffect()
        {
            Shader = new PixelShader();
            Shader.SetStreamSource(new MemoryStream(Convert.FromBase64String(V)));
        }

        public InvertEffect()
        {
            PixelShader = Shader;
            UpdateShaderValue(InputProperty);
        }

        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("Input", typeof(InvertEffect), 0);
    }
}
