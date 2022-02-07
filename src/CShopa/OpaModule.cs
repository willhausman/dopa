using CShopa.Serialization;

namespace CShopa;

public class OpaModule : Disposable, IOpaModule
{
    private readonly IWasmModule module;
    private readonly IOpaSerializer serializer = new DefaultSerializer();

    public OpaModule(IWasmModule module)
    {
        this.module = module;
    }

    public IOpaPolicy CreatePolicy()
    {
        var collection = new BuiltinCollection();
        var runtime = module.CreateRuntime(collection);

        var policy = new OpaPolicy(runtime, serializer, collection);

        // this.builtins.ForEach(b => collection.AddBuiltin(b));

        return policy;
    }

    protected override void DisposeManaged()
    {
        this.module.Dispose();
    }
}
