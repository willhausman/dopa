using DOPA.Extensions;
using DOPA.Runtime;

namespace DOPA.Tests;

public class WasmtimeFixture : Disposable, IRuntimeFixture
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
