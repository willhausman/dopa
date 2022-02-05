namespace Opa.WebAssembly;

public interface IOpaModule : IDisposable
{
    IOpaPolicy CreatePolicy();
}
