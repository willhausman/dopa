using CShopa.Serialization;

namespace CShopa;

public class OpaModule : Disposable, IOpaModule
{
    private readonly IWasmModule module;
    private readonly IOpaSerializer serializer = new DefaultSerializer();

    public OpaModule(IWasmModule module)
    {
        if (module.Exports.Any() && !module.Exports.Contains(WellKnown.Export.opa_eval))
        {
            throw new ArgumentException("The WASM does not export the required 'opa_eval' function from ABI version 1.2.", nameof(module));
        }

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
