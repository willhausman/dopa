namespace DOPA.Exceptions;

/// <summary>
/// The exception called when OpaAbort is called.
/// </summary>
public class OpaAbortException : Exception
{
    private OpaAbortException(string message) 
        : base(message)
    {
    }

    /// <summary>
    /// Initializes the exception with a message.
    /// </summary>
    /// <param name="info">The message.</param>
    /// <returns>An exception instance.</returns>
    public static OpaAbortException Because(string info) => new OpaAbortException($"OPA has aborted with the message: {info}");
}
