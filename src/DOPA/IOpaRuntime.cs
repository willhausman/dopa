using System.Text.Json;
using DOPA.Extensions;
using DOPA.Serialization;

namespace DOPA;

/// <summary>
/// A runtime that can interact with a module instance.
/// </summary>
public interface IOpaRuntime : IOpaDisposable
{
    /// <summary>
    /// Invokes an OPA function on the runtime.
    /// </summary>
    /// <param name="function">The name of the function to run.</param>
    /// <param name="rest">Any parameters needed for the function.</param>
    /// <typeparam name="T">The expected type to return.</typeparam>
    /// <returns>The result of executing the function.</returns>
    T? Invoke<T>(string function, params int[] rest);

    /// <summary>
    /// Invokes an OPA function on the runtime.
    /// </summary>
    /// <param name="function">The name of the function to run.</param>
    /// <param name="rest">Any parameters needed for the function.</param>
    void Invoke(string function, params int[] rest);

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

    /// <summary>
    /// Release shared memory for addresses that are no longer needed.
    /// </summary>
    /// <param name="addresses">The addresses to release.</param>
    void ReleaseMemory(params int[] addresses) =>
        addresses.ForEach(address => Invoke(WellKnown.Export.opa_free, address));

    /// <summary>
    /// Set the heap back to a previous address.
    /// </summary>
    /// <param name="address">The address.</param>
    void ResetHeapTo(int address) =>
        Invoke(WellKnown.Export.opa_heap_ptr_set, address);

    /// <summary>
    /// Get current heap address.
    /// </summary>
    /// <returns>The address.</returns>
    int GetCurrentHeap() =>
        Invoke<int>(WellKnown.Export.opa_heap_ptr_get);

    /// <summary>
    /// Write json into the shared memory buffer, tell the module to load it, then release the shared memory.
    /// </summary>
    /// <param name="json">The json to write.</param>
    /// <returns>The address of the loaded data in the module.</returns>
    int WriteJson(string json)
    {
        var address = WriteValue(json);

        var resultAddress = Invoke<int>(WellKnown.Export.opa_json_parse, address, json.Length);

        ReleaseMemory(address);

        return resultAddress != 0 ? resultAddress : throw new ArgumentException("OPA failed to parse the input json.", nameof(json));
    }

    /// <summary>
    /// Export json to the shared memory buffer from the module, and return the value.
    /// </summary>
    /// <param name="address">The address of the data in the module.</param>
    /// <returns>The json from the shared memory buffer.</returns>
    string ReadJson(int address)
    {
        var jsonAddress = Invoke<int>(WellKnown.Export.opa_json_dump, address);
        var result = ReadValueAt(jsonAddress);

        ReleaseMemory(jsonAddress);

        return result;
    }

    /// <summary>
    /// Invoke a function by name, and read its result.
    /// </summary>
    /// <param name="function">The name of a func export to invoke.</param>
    /// <returns>The result json.</returns>
    string ReadJson(string function)
    {
        var address = Invoke<int>(function);
        var json = ReadJson(address);
        ReleaseMemory(address);
        return json;
    }

    /// <summary>
    /// Gets the map of custom builtins to their ids.
    /// </summary>
    /// <returns>The map of builtins to ids.</returns>
    IDictionary<string, int> GetBuiltins()
    {
        var json = ReadJson(WellKnown.Export.builtins);
        var builtins = JsonSerializer.Deserialize<Dictionary<string, int>>(json, OpaSerializerOptions.Default) ?? new();

        return builtins.WithCaseInsensitiveKeys();
    }

    /// <summary>
    /// Gets the map of entrypoints to their ids.
    /// </summary>
    /// <returns>The map of entrypoints to ids.</returns>
    IDictionary<string, int> GetEntrypoints()
    {
        var json = ReadJson(WellKnown.Export.entrypoints);
        var entrypoints = JsonSerializer.Deserialize<Dictionary<string, int>>(json, OpaSerializerOptions.Default) ?? new();

        return entrypoints.WithCaseInsensitiveKeys();
    }
}
