using System.Text.Json.Serialization;
using OlibKey.Core.Models.StorageModels;

namespace OlibKey.Core.GenerationContexts;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault)]
[JsonSerializable(typeof(Storage))]
public partial class StorageGenerationContext : JsonSerializerContext;