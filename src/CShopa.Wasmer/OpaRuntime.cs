using System.Diagnostics.CodeAnalysis;
using WasmerSharp;

namespace CShopa.Wasmer;

internal sealed class OpaRuntime : Disposable, IOpaRuntime
{
    private Memory memory;
    private Instance instance;

    public OpaRuntime(Memory memory, Instance instance)
    {
        this.memory = memory;
        this.instance = instance;
    }

    public string ReadValueAt(int address) =>
        memory.ReadNullTerminatedString(address);
    
    public void WriteValueAt(int address, string json) =>
        memory.WriteString(address, json);

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
