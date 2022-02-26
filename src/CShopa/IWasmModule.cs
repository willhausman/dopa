namespace CShopa;

public interface IWasmModule : IDisposable
{
    string Name { get; }

    IOpaRuntime CreateRuntime(IBuiltinCollection collection);

    ICollection<string> Exports { get; }
}
