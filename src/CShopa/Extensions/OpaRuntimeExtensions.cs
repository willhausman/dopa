namespace CShopa.Extensions;

public static class OpaRuntimeExtensions
{
    public static int WriteJson(this IOpaRuntime runtime, string json)
    {
        var address = runtime.ReserveMemory(json.Length);
        runtime.WriteValueAt(address, json);

        var resultAddress = runtime.Invoke<int>(WellKnown.Export.opa_json_parse, address, json.Length);

        return resultAddress != 0 ? resultAddress : throw new ArgumentException("OPA failed to parse the input json.", nameof(json));
    }

    public static string ReadJson(this IOpaRuntime runtime, int address)
    {
        var jsonAddress = runtime.Invoke<int>(WellKnown.Export.opa_json_dump, address);
        var result = runtime.ReadValueAt(jsonAddress);

        return result;
    }

    public static int ReserveMemory(this IOpaRuntime runtime, int length) =>
        runtime.Invoke<int>(WellKnown.Export.opa_malloc, length);
}
