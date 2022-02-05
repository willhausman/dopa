namespace Opa.WebAssembly;

public interface IWasmModule : IDisposable
{
    IWasmInstance CreateInstance();
}
