using System.Text.Json;
using CShopa.Extensions;
using CShopa.Serialization;

namespace CShopa;

public interface IOpaRuntime : IOpaDisposable
{
    /// <summary>
    /// Invokes an OPA function on the runtime.
    /// </summary>
    /// <param name="function">The name of the function to run.</param>
    /// <param name="rest">Any parameters needed for the function.</param>
    /// <typeparam name="T">The expected type to return.</typeparam>
    /// <returns>The result of executing the function.</returns>
    T? Invoke<T>(string function, params object[] rest);

    /// <summary>
    /// Invokes an OPA function on the runtime.
    /// </summary>
    /// <param name="function">The name of the function to run.</param>
    /// <param name="rest">Any parameters needed for the function.</param>
    void Invoke(string function, params object[] rest);

    /// <summary>
    /// Reads json from the shared memory buffer.
    /// </summary>
    /// <param name="address">The address to start reading at.</param>
    /// <returns>The json value in shared memory.</returns>
    string ReadValueAt(int address);

    /// <summary>
    /// Writes json to the shared memory buffer.
    /// </summary>
    /// <param name="json">The json to put into the shared memory.</param>
    /// <returns>The address where the value was written.</returns>
    int WriteValue(string json);

    void ReleaseMemory(params int[] addresses) =>
        addresses.ForEach(address => Invoke(WellKnown.Export.opa_free, address));

    void ResetHeapTo(int address) =>
        Invoke(WellKnown.Export.opa_heap_ptr_set, address);

    int GetCurrentHeap() =>
        Invoke<int>(WellKnown.Export.opa_heap_ptr_get);

    int WriteJson(string json)
    {
        var address = WriteValue(json);

        var resultAddress = Invoke<int>(WellKnown.Export.opa_json_parse, address, json.Length);

        ReleaseMemory(address);

        return resultAddress != 0 ? resultAddress : throw new ArgumentException("OPA failed to parse the input json.", nameof(json));
    }

    string ReadJson(int address)
    {
        var jsonAddress = Invoke<int>(WellKnown.Export.opa_json_dump, address);
        var result = ReadValueAt(jsonAddress);

        ReleaseMemory(jsonAddress);

        return result;
    }

    string ReadJson(string function)
    {
        var address = Invoke<int>(function);
        var json = ReadJson(address);
        ReleaseMemory(address);
        return json;
    }

    IDictionary<string, int> GetBuiltins()
    {
        var json = ReadJson(WellKnown.Export.builtins);
        var builtins = JsonSerializer.Deserialize<Dictionary<string, int>>(json, OpaSerializerOptions.Default) ?? new();

        return builtins.WithCaseInsensitiveKeys();
    }

    IDictionary<string, int> GetEntrypoints()
    {
        var json = ReadJson(WellKnown.Export.entrypoints);
        var entrypoints = JsonSerializer.Deserialize<Dictionary<string, int>>(json, OpaSerializerOptions.Default) ?? new();

        return entrypoints.WithCaseInsensitiveKeys();
    }
}
