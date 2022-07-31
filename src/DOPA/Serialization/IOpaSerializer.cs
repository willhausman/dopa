namespace DOPA.Serialization;

/// <summary>
/// A serializer configured to read and write json with an <see cref="IOpaPolicy" />.
/// </summary>
public interface IOpaSerializer
{
    /// <summary>
    /// Serialize a value to json.
    /// </summary>
    /// <param name="value">The value to serialize.</param>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <returns>The serialized string.</returns>
    string Serialize<TValue>(TValue value);

    /// <summary>
    /// Deserialize some json to a type.
    /// </summary>
    /// <param name="json">The json value.</param>
    /// <typeparam name="TValue">The expected type.</typeparam>
    /// <returns>The deserialized value.</returns>
    TValue? Deserialize<TValue>(string json);
}
