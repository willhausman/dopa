using System.Text.Json;
using CShopa.Serialization;

namespace CShopa.Extensions;

public static class IOpaRuntimeExtensions
{
    public static int WriteJson(this IOpaRuntime runtime, string json)
    {
        var address = runtime.WriteValue(json);

        var resultAddress = runtime.Invoke<int>(WellKnown.Export.opa_json_parse, address, json.Length);

        runtime.ReleaseMemory(address);

        return resultAddress != 0 ? resultAddress : throw new ArgumentException("OPA failed to parse the input json.", nameof(json));
    }

    public static string ReadJson(this IOpaRuntime runtime, int address)
    {
        var jsonAddress = runtime.Invoke<int>(WellKnown.Export.opa_json_dump, address);
        var result = runtime.ReadValueAt(jsonAddress);

        runtime.ReleaseMemory(jsonAddress);

        return result;
    }

    public static string ReadJson(this IOpaRuntime runtime, string function)
    {
        var address = runtime.Invoke<int>(function);
        var json = runtime.ReadJson(address);
        runtime.ReleaseMemory(address);
        return json;
    }

    public static IDictionary<string, int> GetBuiltins(this IOpaRuntime runtime)
    {
        var json = runtime.ReadJson(WellKnown.Export.builtins);
        var builtins = JsonSerializer.Deserialize<Dictionary<string, int>>(json, OpaSerializerOptions.Default) ?? new();

        return builtins.WithCaseInsensitiveKeys();
    }

    public static IDictionary<string, int> GetEntrypoints(this IOpaRuntime runtime)
    {
        var json = runtime.ReadJson(WellKnown.Export.entrypoints);
        var entrypoints = JsonSerializer.Deserialize<Dictionary<string, int>>(json, OpaSerializerOptions.Default) ?? new();

        return entrypoints.WithCaseInsensitiveKeys();
    }
    
    public static void ReleaseMemory(this IOpaRuntime runtime, params int[] addresses) =>
        addresses.ForEach(address => runtime.Invoke(WellKnown.Export.opa_free, address));

    public static void ResetHeapTo(this IOpaRuntime runtime, int address) =>
        runtime.Invoke(WellKnown.Export.opa_heap_ptr_set, address);
    
    public static int GetCurrentHeap(this IOpaRuntime runtime) =>
        runtime.Invoke<int>(WellKnown.Export.opa_heap_ptr_get);
}
