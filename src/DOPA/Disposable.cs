namespace DOPA;

public interface IOpaDisposable : IDisposable
{
    bool Disposed { get; }
}

public abstract class Disposable : IOpaDisposable
{
    public bool Disposed { get; private set; }

    protected virtual void DisposeManaged() { }

    /// <summary>
    /// If you override this method, call Dispose(false) in your class finalizer.
    /// </summary>
    protected virtual void DisposeUnmanaged() { }

    protected void Dispose(bool disposing)
    {
        if (!Disposed)
        {
            if (disposing)
            {
                DisposeManaged();
            }

            DisposeUnmanaged();

            Disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
