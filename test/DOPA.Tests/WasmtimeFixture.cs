using DOPA.Extensions;
using DOPA.Loader;

namespace DOPA.Tests;

public class WasmtimeFixture : Disposable, IRuntimeFixture
{
    private readonly Lazy<IOpaModule> exampleModule = new Lazy<IOpaModule>(() =>
    {
        using var stream = OpaLoader.CreateWebAssemblyStream("policies/example.rego", "example/hello").Result;
        return WasmModule.FromStream("example", stream);
    });
    public IOpaModule ExampleModule => exampleModule.Value;

    private readonly Lazy<IOpaModule> builtinsModule = new Lazy<IOpaModule>(() =>
    {
        var args = new OpaArguments("policies/builtins.rego", "builtins")
        {
            Capabilities = "policies/builtins.capabilities.json"
        };
        using var stream = OpaLoader.CreateWebAssemblyStream(args).Result;
        return WasmModule.FromStream("builtins", stream);
    });
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
