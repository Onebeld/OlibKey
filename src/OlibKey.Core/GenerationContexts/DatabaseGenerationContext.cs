using System.Text.Json.Serialization;
using OlibKey.Core.Models.DatabaseModels;

namespace OlibKey.Core.GenerationContexts;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault)]
[JsonSerializable(typeof(Database))]
public partial class DatabaseGenerationContext : JsonSerializerContext;