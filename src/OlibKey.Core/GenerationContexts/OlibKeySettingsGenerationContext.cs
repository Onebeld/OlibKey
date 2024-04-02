using System.Text.Json.Serialization;
using OlibKey.Core.Settings;

namespace OlibKey.Core.GenerationContexts;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(OlibKeySettings))]
internal partial class OlibKeySettingsGenerationContext : JsonSerializerContext;