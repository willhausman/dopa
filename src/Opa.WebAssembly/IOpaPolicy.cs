namespace Opa.WebAssembly;

public interface IOpaPolicy : IDisposable
{
    T? Evaluate<T>(object input);

    void SetData<T>(T input);
}
