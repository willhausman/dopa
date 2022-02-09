using CShopa.Extensions;
using CShopa.Wasmer;

namespace CShopa.Tests;

public class WasmerFixture : Disposable, IRuntimeFixture
{
    private Lazy<IOpaModule> exampleModule = new Lazy<IOpaModule>(() => WasmModule.FromFile("policies/example.wasm"));
    public IOpaModule ExampleModule => exampleModule.Value;

    private Lazy<IOpaModule> builtinsModule = new Lazy<IOpaModule>(() => WasmModule.FromFile("policies/builtins.wasm"));
    public IOpaModule BuiltinsModule => builtinsModule.Value;

    protected override void DisposeManaged()
    {
        var lazies = new[] { exampleModule, builtinsModule };

        lazies.ForEach(m =>
        {
            if (m.IsValueCreated)
            {
                m.Value.Dispose();
            }
        });
    }
}
