using System.Text.Json;

namespace Opa.WebAssembly.Serialization;

public static class OpaSerializerOptions
{
    private static readonly Lazy<JsonSerializerOptions> @default = new Lazy<JsonSerializerOptions>(() =>
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        });
    
    public static JsonSerializerOptions Default = @default.Value;
}
