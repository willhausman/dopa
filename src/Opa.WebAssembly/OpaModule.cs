namespace Opa.WebAssembly;

public class OpaModule : Disposable, IOpaModule
{
    private readonly IWasmModule module;

    public OpaModule(IWasmModule module)
    {
        this.module = module;
    }

    public IOpaPolicy CreatePolicy()
    {
        throw new NotImplementedException();
    }

    protected override void DisposeManaged()
    {
        this.module.Dispose();
    }
}
