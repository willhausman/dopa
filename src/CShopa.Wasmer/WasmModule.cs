using WasmerSharp;

namespace CShopa.Wasmer;

public class WasmModule : Disposable, IWasmModule
{
    private readonly Module module;

    // WasmerSharp throws an exception trying to get its exports. Really, this is just POC for using another runtime.
    public ICollection<string> Exports => new[] { WellKnown.Export.opa_eval };

    private WasmModule(Module  module)
    {
        this.module = module;
    }

    public IOpaRuntime CreateRuntime(IBuiltinCollection collection)
    {
        var memory = Memory.Create(WellKnown.Requirements.MinimumMemorySize);
        var instance = module.Instatiate(Imports.ForOpa(memory, collection));

        return new OpaRuntime(memory, instance);
    }

    public static IOpaModule FromFile(string filePath)
    {
        var wasm = File.ReadAllBytes(filePath);
        var module = new WasmModule(Module.Create(wasm));
        return new OpaModule(module);
    }

    protected override void DisposeManaged()
    {
        module.Dispose();
    }
}
