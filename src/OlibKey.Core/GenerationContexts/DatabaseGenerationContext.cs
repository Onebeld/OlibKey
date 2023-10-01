using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using OlibKey.Core.Structures;

namespace OlibKey.Core;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(Database))]
public partial class DatabaseGenerationContext : JsonSerializerContext
{
    static DatabaseGenerationContext()
    {
        if (Default.GeneratedSerializerOptions == null)
            return;

        Default.GeneratedSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers = { ExcludeEmptyAndNullValues }
        };
    }

    static void ExcludeEmptyAndNullValues(JsonTypeInfo jsonTypeInfo)
    {
        if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
            return;

        foreach (JsonPropertyInfo jsonPropertyInfo in jsonTypeInfo.Properties)
        {
            if (jsonPropertyInfo.PropertyType == typeof(string))
            {
                jsonPropertyInfo.ShouldSerialize = static (_, value) =>
                    !string.IsNullOrEmpty((string)value!) || value is "0" or "false";
            }
            else
                jsonPropertyInfo.ShouldSerialize = static (_, value) => value is not null;
        }
    }
}