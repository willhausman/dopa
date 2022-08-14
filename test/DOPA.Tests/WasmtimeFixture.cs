using DOPA.Extensions;
using DOPA.Loader;

namespace DOPA.Tests;

public class WasmtimeFixture : Disposable, IRuntimeFixture
{
    private readonly Lazy<IOpaModule> exampleModule = new Lazy<IOpaModule>(() =>
    {
        using var stream = OpaLoader.CreateWebAssemblyStream("policies/example.rego", "example/hello");
        return WasmModule.FromStream("example", stream.Result);
    });
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
