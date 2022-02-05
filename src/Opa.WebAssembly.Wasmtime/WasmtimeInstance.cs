namespace Opa.WebAssembly.Wasmtime;

internal class WasmtimeInstance : IWasmInstance
{
    private readonly Runtime runtime;

    public WasmtimeInstance(Runtime runtime)
    {
        this.runtime = runtime;
    }

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                this.runtime.Dispose();
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
