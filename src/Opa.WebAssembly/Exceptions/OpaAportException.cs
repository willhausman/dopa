namespace Opa.WebAssembly.Exceptions;

public class OpaAbortException : Exception
{
    private OpaAbortException(string message) 
        : base(message)
    {
    }

    public static OpaAbortException Because(string info) => new OpaAbortException($"OPA has aported with the message: {info}");
}
