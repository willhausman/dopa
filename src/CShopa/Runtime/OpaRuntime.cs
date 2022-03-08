using System.Diagnostics.CodeAnalysis;
using Wasmtime;

namespace CShopa.Runtime;

internal sealed class OpaRuntime : Disposable, IOpaRuntime
{
    private readonly Store store;

    [NotNull]
    private Instance? instance;

    [NotNull]
    private Memory? memory;

    public OpaRuntime(Store store, Memory memory, Instance instance)
    {
        this.store = store;
        this.memory = memory;
        this.instance = instance;
    }

    public string ReadValueAt(int address) =>
        memory.ReadNullTerminatedString(store, address);

    public int WriteValue(string json)
    {
        var address = Invoke<int>(WellKnown.Export.opa_malloc, json.Length);
        memory.WriteString(store, address, json);
        return address;
    }

    public void Invoke(string function, params object[] rest)
    {
        var run = instance.GetFunction(store, function);

        if (run is not null)
        {
            run.Invoke(store, rest);
        }
        else
        {
            throw new InvalidOperationException($"Could not invoke '{function}'.");
        }
    }

    public T? Invoke<T>(string function, params object[] rest)
    {
        var run = instance.GetFunction(store, function);

        if (run is not null)
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
