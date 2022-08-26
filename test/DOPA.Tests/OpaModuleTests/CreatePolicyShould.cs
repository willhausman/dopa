using FluentAssertions;
using Xunit;

namespace DOPA.Tests.OpaModuleTests;

public class CreatePolicyShould : OpaModuleTestBase
{
    public CreatePolicyShould(RuntimeFixture fixture)
        : base(fixture)
    {
    }

    [Theory]
    [InlineData(Runtime.Wasmtime)]
    public void ReturnAPolicy(Runtime runtime)
    {
        var module = ExampleModule(runtime);

        using var policy = module.CreatePolicy();

        policy.Should().NotBeNull();
    }

    [Theory]
    [InlineData(Runtime.Wasmtime)]
    public void ReturnAcrossThreads(Runtime runtime)
    {
        var module = ExampleModule(runtime);
        var loop = new int[500];

        Parallel.ForEach(loop, _ =>
        {
            using var policy = module.CreatePolicy();
            policy.SetData(new { world = "hello" });
            var result = policy.Evaluate<bool>(new { message = "hello" });
            result.Should().BeTrue();
        });
    }
}
