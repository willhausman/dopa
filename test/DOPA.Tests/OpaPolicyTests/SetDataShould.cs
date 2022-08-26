using FluentAssertions;
using Xunit;

namespace DOPA.Tests.OpaPolicyTests;

public class SetDataShould : OpaPolicyTestBase
{
    public SetDataShould(RuntimeFixture fixture)
        : base(fixture)
    {
    }

    [Theory]
    [InlineData(Runtime.Wasmtime)]
    public void OverwritePreviousData(Runtime runtime)
    {
        using var policy = ExamplePolicy(runtime);
        policy.SetData(new { some="other object" });
        policy.SetData(new { world = "hello" });

        var result = policy.Evaluate<bool>(new { message = "hello" });

        result.Should().BeTrue();
        policy.Data.Should().BeEquivalentTo(new { world = "hello" });
        policy.DataJson.Should().Be(@"{""world"":""hello""}");
    }
}
