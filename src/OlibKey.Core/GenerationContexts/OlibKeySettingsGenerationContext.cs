﻿using System.Text.Json.Serialization;

namespace OlibKey.Core.GenerationContexts;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(OlibKeySettings))]
internal partial class OlibKeySettingsGenerationContext : JsonSerializerContext;