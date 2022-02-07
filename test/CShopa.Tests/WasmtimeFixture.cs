using CShopa.Wasmtime;

namespace CShopa.Tests;

public class WasmtimeFixture : Disposable, IRuntimeFixture
{
    private Lazy<IOpaModule> exampleModule = new Lazy<IOpaModule>(() => WasmModule.FromFile("policies/example.wasm"));
    public IOpaModule ExampleModule => exampleModule.Value;

    protected override void DisposeManaged()
    {
        if (exampleModule.IsValueCreated)
        {
            ExampleModule.Dispose();
        }
    }
}
