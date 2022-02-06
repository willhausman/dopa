using Wasmtime;

namespace Opa.WebAssembly.Wasmtime;

public class WasmModule : Disposable, IWasmModule
{
    private readonly Engine engine;
    private readonly Module module;

    private WasmModule(Engine engine, Module  module)
    {
        this.engine = engine;
        this.module = module;
    }

    public IOpaRuntime CreateRuntime() => new OpaRuntime(engine, module);

    public static IOpaModule FromFile(string filePath)
    {
        var engine = new Engine();
        var module = new WasmModule(engine, Module.FromFile(engine, filePath));
        return new OpaModule(module);
    }

    protected override void DisposeManaged()
    {
        module.Dispose();
        engine.Dispose();
    }
}
