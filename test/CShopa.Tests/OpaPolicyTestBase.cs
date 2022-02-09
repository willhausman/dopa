using Xunit;

namespace CShopa.Tests;

[Collection(nameof(OpaPolicyCollection))]
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
