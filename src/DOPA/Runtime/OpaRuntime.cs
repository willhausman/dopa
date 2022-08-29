using System.Diagnostics.CodeAnalysis;
using Wasmtime;

namespace DOPA.Runtime;

/// <summary>
/// An <see cref="IOpaRuntime" /> using Wasmtime.
/// </summary>
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
        memory.ReadNullTerminatedString(address);

    public int WriteValue(string json)
    {
        var address = Invoke<int>(WellKnown.Export.opa_malloc, json.Length);
        memory.WriteString(address, json);
        return address;
    }

    public void Invoke(string function, params int[] rest)
    {
        var run = instance.GetFunction(function);

        var @params = rest
            .ToList()
            .ConvertAll<ValueBox>(p => p)
            .ToArray();

        if (run is not null)
        {
            run.Invoke(@params);
        }
        else
        {
            throw new InvalidOperationException($"Could not invoke '{function}'.");
        }
    }

    public T? Invoke<T>(string function, params int[] rest)
    {
        var run = instance.GetFunction(function);

        var @params = rest
            .ToList()
            .ConvertAll<ValueBox>(p => p)
            .ToArray();

        if (run is not null)
        {
            return (T?)run.Invoke(@params);
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
