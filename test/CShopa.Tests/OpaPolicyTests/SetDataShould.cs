using FluentAssertions;
using Xunit;

namespace CShopa.Tests.OpaPolicyTests;

public class SetDataShould : OpaPolicyTestBase
{
    public SetDataShould(RuntimeFixture fixture)
        : base(fixture)
    {
    }

    [Theory]
    [InlineData(Runtime.Wasmtime)]
    [InlineData(Runtime.Wasmer)]
    public void OverwritePreviousData(Runtime runtime)
    {
        var policy = ExamplePolicy(runtime);
        policy.SetData(new { some="other object" });
        policy.SetData(new { world = "hello" });

        var result = policy.Evaluate<bool>(new { message = "hello" });

        result.Should().BeTrue();
    }
}
