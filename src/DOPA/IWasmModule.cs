namespace DOPA;

public interface IWasmModule : IOpaDisposable
{
    string Name { get; }

    IOpaRuntime CreateRuntime(IBuiltinCollection collection);

    ICollection<string> Exports { get; }
}
