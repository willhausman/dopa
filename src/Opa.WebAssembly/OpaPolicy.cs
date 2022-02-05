namespace Opa.WebAssembly;

using Serialization;

public class OpaPolicy : Disposable, IOpaPolicy
{
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

    protected override void DisposeManaged()
    {
        this.instance.Dispose();
    }
}
