using Wasmtime;

namespace Opa.WebAssembly.Wasmtime;

public class WasmtimeModule : Disposable, IWasmModule
{
    private readonly Engine engine;
    private readonly Module module;

    private WasmtimeModule(Engine engine, Module  module)
    {
        this.engine = engine;
        this.module = module;
    }

    public IOpaRuntime CreateRuntime() => new Runtime(engine, module);

    public static IOpaModule FromFile(string filePath)
    {
        var engine = new Engine();
        var module = new WasmtimeModule(engine, Module.FromFile(engine, filePath));
        return new OpaModule(module);
    }

    protected override void DisposeManaged()
    {
        module.Dispose();
        engine.Dispose();
    }
}
