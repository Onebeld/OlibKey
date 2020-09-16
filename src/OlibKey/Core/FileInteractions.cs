using System;
using System.IO;

namespace OlibKey.Core
{
    public static class FileInteractions
    {
        public static string ImportFile(string path) => Convert.ToBase64String(File.ReadAllBytes(path));

        public static void ExportFile(string file, string path) => File.WriteAllBytes(path, Convert.FromBase64String(file));
    }
}
