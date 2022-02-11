using Xunit;

namespace CShopa.Tests.OpaModuleTests;

[Collection(nameof(RuntimeCollection))]
public abstract class OpaModuleTestBase
{
    private readonly RuntimeFixture fixture;

    protected OpaModuleTestBase(RuntimeFixture fixture)
    {
        this.fixture = fixture;
    }

    protected IOpaModule ExampleModule(Runtime runtime) => fixture.ExampleModule(runtime);
}
