using System.Text.Json;

namespace CShopa.Serialization;

public class OpaSerializer : IOpaSerializer
{
    private readonly JsonSerializerOptions options;

    public OpaSerializer()
    {
        this.options = OpaSerializerOptions.Default;
    }

    public OpaSerializer(JsonSerializerOptions options)
    {
        this.options = options;
    }

    public TValue? Deserialize<TValue>(string json) =>
        JsonSerializer.Deserialize<TValue>(json, this.options);

    public string Serialize<TValue>(TValue value) =>
        JsonSerializer.Serialize(value, this.options);
}
