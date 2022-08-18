namespace DOPA.Cli;

/// <summary>
/// A disposable resource.
/// </summary>
public interface IOpaDisposable : IDisposable
{
    /// <summary>
    /// Whether or not this resource has already been disposed.
    /// </summary>
    bool Disposed { get; }
}

/// <inheritdoc />
public abstract class Disposable : IOpaDisposable
{
    /// <inheritdoc/>
    
    public bool Disposed { get; private set; }

    /// <summary>
    /// Override to dispose other managed resources.
    /// </summary>
    protected virtual void DisposeManaged() { }

    /// <summary>
    /// Override to dispose unmanaged resources.
    /// </summary>
    /// <remarks>
    /// If you override this method, call Dispose(false) in your class finalizer.
    /// </remarks>
    protected virtual void DisposeUnmanaged() { }

    /// <summary>
    /// Implement a dispose pattern.
    /// </summary>
    /// <param name="disposing">Disposing or finalizing.</param>
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

    /// <inheritdoc/>

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
