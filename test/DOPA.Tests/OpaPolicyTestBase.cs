using Xunit;

namespace DOPA.Tests;

[Collection(nameof(RuntimeCollection))]
public abstract class OpaPolicyTestBase
{
    private readonly RuntimeFixture fixture;

    protected OpaPolicyTestBase(RuntimeFixture fixture)
    {
        this.fixture = fixture;
    }

    protected IOpaPolicy ExamplePolicy(Runtime runtime) => fixture.ExampleModule(runtime).CreatePolicy();

    protected IOpaPolicy BuiltinsPolicy(Runtime runtime) => fixture.BuiltinsModule(runtime).CreatePolicy();
}
