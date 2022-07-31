using System.Text.Json;

namespace DOPA.Serialization;

/// <inheritdoc />
public class OpaSerializer : IOpaSerializer
{
    private readonly JsonSerializerOptions options;

    /// <summary>
    /// Initializes the class with default options.
    /// </summary>
    public OpaSerializer()
        : this(OpaSerializerOptions.Default)
    {
    }

    /// <summary>
    /// Initializes the class.
    /// </summary>
    /// <param name="options">The options to use when serializing.</param>
    public OpaSerializer(JsonSerializerOptions options)
    {
        this.options = options;
    }

    /// <inheritdoc/>
    public TValue? Deserialize<TValue>(string json) =>
        JsonSerializer.Deserialize<TValue>(json, this.options);

    /// <inheritdoc/>
    public string Serialize<TValue>(TValue value) =>
        JsonSerializer.Serialize(value, this.options);
}
