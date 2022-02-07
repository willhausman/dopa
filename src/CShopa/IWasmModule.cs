namespace CShopa;

public interface IWasmModule : IDisposable
{
    IOpaRuntime CreateRuntime(IBuiltinCollection collection);
}
