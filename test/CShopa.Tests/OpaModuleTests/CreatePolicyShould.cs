using FluentAssertions;
using Xunit;

namespace CShopa.Tests.OpaModuleTests;

public class CreatePolicyShould : OpaModuleTestBase
{
    public CreatePolicyShould(RuntimeFixture fixture)
        : base(fixture)
    {
    }

    [Theory]
    [InlineData(Runtime.Wasmtime)]
    [InlineData(Runtime.Wasmer)]
    public void ReturnAPolicy(Runtime runtime)
    {
        using var module = ExampleModule(runtime);

        using var policy = module.CreatePolicy();

        policy.Should().NotBeNull();
    }
}
