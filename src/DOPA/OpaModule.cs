using DOPA.Builtins;
using DOPA.Serialization;

namespace DOPA;

/// <inheritdoc />
public class OpaModule : Disposable, IOpaModule
{
    private readonly IWasmModule module;

    /// <summary>
    /// Initializes the class.
    /// </summary>
    /// <param name="module">The WebAssembly module used to instantiate policies.</param>
    public OpaModule(IWasmModule module)
    {
        if (!module.Exports.Contains(WellKnown.Export.opa_eval))
        {
            throw new ArgumentException("The WASM does not export the required 'opa_eval' function from ABI version 1.2.", nameof(module));
        }

        this.module = module;
    }

    /// <inheritdoc/>
    public string Name => module.Name;

    /// <inheritdoc/>
    public IOpaSerializer Serializer { get; set; } = new OpaSerializer();

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    protected override void DisposeManaged()
    {
        this.module.Dispose();
    }
}
