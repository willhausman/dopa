using DOPA.Extensions;
using DOPA.Wasmer;

namespace DOPA.Tests;

public class WasmerFixture : Disposable, IRuntimeFixture
{
    private readonly Lazy<IOpaModule> exampleModule = new Lazy<IOpaModule>(() => WasmModule.FromFile("policies/example.wasm"));
    public IOpaModule ExampleModule => exampleModule.Value;

    private readonly Lazy<IOpaModule> builtinsModule = new Lazy<IOpaModule>(() => WasmModule.FromFile("policies/builtins.wasm"));
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
