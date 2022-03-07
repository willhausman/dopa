namespace CShopa.Exceptions;

public class OpaAbortException : Exception
{
    private OpaAbortException(string message) 
        : base(message)
    {
    }

    public static OpaAbortException Because(string info) => new OpaAbortException($"OPA has aborted with the message: {info}");
}
