using Opa.WebAssembly.Serialization;

namespace Opa.WebAssembly.Wasmtime;
public class OpaPolicy : IOpaPolicy
{
    private bool disposedValue;

    private readonly IWasmInstance instance;
    private readonly IOpaSerializer serializer;

    public OpaPolicy(IWasmInstance instance, IOpaSerializer serializer)
    {
        this.instance = instance;
        this.serializer = serializer;
    }

    public T? Evaluate<T>(object input)
    {
        throw new NotImplementedException();
    }

    public void SetData<T>(T input)
    {
        throw new NotImplementedException();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                this.instance.Dispose();
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
