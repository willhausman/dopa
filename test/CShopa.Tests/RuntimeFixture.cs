using CShopa.Extensions;

namespace CShopa.Tests;

public class RuntimeFixture : Disposable
{
    private Dictionary<Runtime, IRuntimeFixture> fixtures = new()
    {
        { Runtime.Wasmtime, new WasmtimeFixture() }
    };

    public IOpaModule ExampleModule(Runtime runtime) => fixtures[runtime].ExampleModule;

    protected override void DisposeManaged()
    {
        fixtures.Values.ForEach(f => f.Dispose());
    }
}
