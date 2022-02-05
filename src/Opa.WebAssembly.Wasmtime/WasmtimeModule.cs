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

    public IWasmInstance CreateInstance()
    {
        var runtime = new PolicyRuntime(engine);
        return new WasmtimeInstance(runtime, module);
    }

      public static WasmtimeModule FromFile(string filePath)
      {
          var engine = new Engine();
          return new WasmtimeModule(engine, Module.FromFile(engine, filePath));
      }

    protected override void DisposeManaged()
    {
        module.Dispose();
        engine.Dispose();
    }
}
