using CShopa.Extensions;

namespace CShopa;

public abstract class OpaRuntimeBase : Disposable, IOpaRuntime
{
    public int WriteJson(string json)
    {
        var address = ReserveMemory(json.Length);
        WriteValueAt(address, json);

        var resultAddress = Invoke<int>(WellKnown.Export.opa_json_parse, address, json.Length);

        ReleaseMemory(address);

        return resultAddress != 0 ? resultAddress : throw new ArgumentException("OPA failed to parse the input json.", nameof(json));
    }

    public string ReadJson(int address, bool releaseAddress = true)
    {
        var jsonAddress = Invoke<int>(WellKnown.Export.opa_json_dump, address);
        var result = ReadValueAt(jsonAddress);

        ReleaseMemory(jsonAddress);

        if (releaseAddress)
        {
            ReleaseMemory(address);
        }

        return result;
    }

    public void ReleaseMemory(params int[] addresses) =>
        addresses.ForEach(a => Invoke(WellKnown.Export.opa_free, a));

    public int ReserveMemory(int length) =>
        Invoke<int>(WellKnown.Export.opa_malloc, length);

    public abstract T? Invoke<T>(string function, params object[] rest);
    public abstract void Invoke(string function, params object[] rest);
    public abstract string ReadValueAt(int address);
    public abstract void WriteValueAt(int address, string json);
}
