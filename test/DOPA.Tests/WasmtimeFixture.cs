using DOPA.Cli;
using DOPA.Extensions;

namespace DOPA.Tests;

public class WasmtimeFixture : Disposable, IRuntimeFixture
{
    private readonly Lazy<IOpaModule> exampleModule = new Lazy<IOpaModule>(() =>
    {
        using var bundle = Opa.Build.WebAssembly().Files("policies/example.rego").Entrypoints("example/hello").Execute();
        using var stream = bundle.ExtractWebAssemblyModule();
        return WasmModule.FromStream("example", stream);
    });
    public IOpaModule ExampleModule => exampleModule.Value;

    private readonly Lazy<IOpaModule> builtinsModule = new Lazy<IOpaModule>(() =>
    {
        using var bundle = Opa.Build
            .WebAssembly()
            .Files("policies/builtins.rego")
            .Entrypoints("builtins")
            .Capabilities("policies/builtins.capabilities.json")
            .Execute();
        using var stream = bundle.ExtractWebAssemblyModule();
        return WasmModule.FromStream("example", stream);
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
