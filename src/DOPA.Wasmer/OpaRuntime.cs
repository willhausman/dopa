using WasmerSharp;

namespace DOPA.Wasmer;

internal sealed class OpaRuntime : Disposable, IOpaRuntime
{
    private const double PageSize = 65536; // bytes size of a WebAssembly memory page
    private readonly Memory memory;
    private readonly Instance instance;

    public OpaRuntime(Memory memory, Instance instance)
    {
        this.memory = memory;
        this.instance = instance;
    }

    public string ReadValueAt(int address) =>
        memory.ReadNullTerminatedString(address);

    public int WriteValue(string json)
    {
        var address = Invoke<int>(WellKnown.Export.opa_malloc, json.Length);
        memory.WriteString(address, json);
        return address;
    }

    public void Invoke(string function, params object[] rest)
    {
        var _ = instance.Call(function, rest) ?? throw new InvalidOperationException($"Could not invoke '{function}'.");
    }

    public T? Invoke<T>(string function, params object[] rest)
    {
        var result = instance.Call(function, rest) ?? throw new InvalidOperationException($"Could not invoke '{function}'.");
        return (T?)result[0];
    }

    protected override void DisposeManaged()
    {
        this.memory.Dispose();
        this.instance.Dispose();
    }
}
