namespace Opa.WebAssembly.Wasmtime;

public class OpaModule : IOpaModule
{
    private bool disposedValue;
    private readonly IWasmModule module;

    public OpaModule(IWasmModule module)
    {
        this.module = module;
    }

    public IOpaPolicy CreatePolicy()
    {
        throw new NotImplementedException();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                this.module.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
