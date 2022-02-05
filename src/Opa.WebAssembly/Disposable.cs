namespace Opa.WebAssembly;

public abstract class Disposable : IDisposable
{
    private bool disposedValue;

    protected virtual void DisposeManaged() { }

    /// <summary>
    /// If you override this method, call Dispose(false) in your class finalizer.
    /// </summary>
    protected virtual void DisposeUnmanaged() { }

    protected void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                DisposeManaged();
            }

            DisposeUnmanaged();

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
