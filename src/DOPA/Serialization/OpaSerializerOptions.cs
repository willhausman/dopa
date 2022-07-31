using System.Text.Json;

namespace DOPA.Serialization;

/// <summary>
/// Some serializer options.
/// </summary>
public static class OpaSerializerOptions
{
    private static readonly Lazy<JsonSerializerOptions> @default = new Lazy<JsonSerializerOptions>(() =>
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        });

    /// <summary>
    /// The default serializer options.
    /// </summary>
    public static JsonSerializerOptions Default => @default.Value;
}
