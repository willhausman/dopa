using Wasmtime;

namespace Opa.WebAssembly.Wasmtime;

public class WasmtimeModule : IWasmModule
{
    private bool disposedValue;
    private readonly Engine engine;
    private readonly Module module;

    private WasmtimeModule(Engine engine, Module  module)
    {
        this.engine = engine;
        this.module = module;
    }

    public IWasmInstance CreateInstance()
    {
        throw new NotImplementedException();
    }

      public static WasmtimeModule FromFile(string filePath)
      {
          var engine = new Engine();
          return new WasmtimeModule(engine, Module.FromFile(engine, filePath));
      }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                engine.Dispose();
                module.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
