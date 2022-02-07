using System.Diagnostics.CodeAnalysis;
using CShopa.Extensions;
using Wasmtime;

namespace CShopa.Wasmtime;

internal sealed class OpaRuntime : Disposable, IOpaRuntime
{
    private readonly Store store;

    [NotNull]
    private Instance? instance;

    [NotNull]
    private Memory? memory;

    public OpaRuntime(Store store, Memory memory, Linker linker, Module module)
    {
        this.store = store;
        this.memory = memory;

        instance = linker.Instantiate(store, module);
    }

    public int WriteJson(string json)
    {
        var address = ReserveMemory(json.Length);
        WriteValueAt(address, json);

        var resultAddress = Invoke<int>(WellKnown.Export.opa_json_parse, address, json.Length);

        ReleaseMemory(address);

        return resultAddress != 0 ? resultAddress : throw new ArgumentException("OPA failed to parse the input json.", nameof(json));
    }

    public string ReadValueAt(int address) =>
        memory.ReadNullTerminatedString(store, address);
    
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
    
    public void WriteValueAt(int address, string json)
    {
        memory.WriteString(store, address, json);
    }

    public void Invoke(string function, params object[] rest)
    {
        var run = instance.GetFunction(store, function);

        if (run is Function)
        {
            run.Invoke(store, rest);
        }
        else
        {
            throw new InvalidOperationException($"Could not invoke '{function}' builtin.");
        }
    }

    public T? Invoke<T>(string function, params object[] rest)
    {
        var run = instance.GetFunction(store, function);

        if (run is Function)
        {
            return (T?)run.Invoke(store, rest);
        }

        throw new InvalidOperationException($"Could not invoke '{function}'");
    }

    ~OpaRuntime()
    {
        Dispose(false);
    }

    protected override void DisposeManaged()
    {
        this.store.Dispose();
    }

    protected override void DisposeUnmanaged()
    {
        this.instance = null;
        this.memory = null;
    }
}
