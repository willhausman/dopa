using CShopa.Extensions;

namespace CShopa.Tests;

public class RuntimeFixture : Disposable
{
    private readonly Dictionary<Runtime, IRuntimeFixture> fixtures = new()
    {
        { Runtime.Wasmtime, new WasmtimeFixture() },
        { Runtime.Wasmer, new WasmerFixture() },
    };

    public IOpaModule ExampleModule(Runtime runtime) => fixtures[runtime].ExampleModule;

    public IOpaModule BuiltinsModule(Runtime runtime) => fixtures[runtime].BuiltinsModule;

    protected override void DisposeManaged()
    {
        fixtures.Values.ForEach(f => f.Dispose());
    }
}
