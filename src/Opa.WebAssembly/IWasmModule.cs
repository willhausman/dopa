namespace Opa.WebAssembly;

public interface IWasmModule : IDisposable
{
    IOpaRuntime CreateRuntime();
}
