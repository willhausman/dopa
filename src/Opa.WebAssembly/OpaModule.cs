using Opa.WebAssembly.Serialization;

namespace Opa.WebAssembly;

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
        var runtime = module.CreateRuntime();
        return new OpaPolicy(runtime, serializer);
    }

    protected override void DisposeManaged()
    {
        this.module.Dispose();
    }
}
