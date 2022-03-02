using CShopa.Builtins;
using CShopa.Serialization;

namespace CShopa;

public class OpaModule : Disposable, IOpaModule
{
    private readonly IWasmModule module;

    public OpaModule(IWasmModule module)
    {
        if (!module.Exports.Contains(WellKnown.Export.opa_eval))
        {
            throw new ArgumentException("The WASM does not export the required 'opa_eval' function from ABI version 1.2.", nameof(module));
        }

        this.module = module;
    }

    public string Name => module.Name;

    public IOpaSerializer Serializer { get; set; } = new OpaSerializer();

    public IOpaPolicy CreatePolicy()
    {
        var builtins = new BuiltinCollection();
        var runtime = module.CreateRuntime(builtins);
        var entrypoints = new EntrypointCollection(runtime.GetEntrypoints());
        builtins.ConfigureBuiltinIds(runtime.GetBuiltins());

        // this.builtins.ForEach(b => collection.AddBuiltin(b));

        var policy = new OpaPolicy(runtime, Serializer, builtins, entrypoints);

        return policy;
    }

    protected override void DisposeManaged()
    {
        this.module.Dispose();
    }
}
