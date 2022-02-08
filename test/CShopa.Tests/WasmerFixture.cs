using CShopa.Wasmer;

namespace CShopa.Tests;

public class WasmerFixture : Disposable, IRuntimeFixture
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
