using FluentAssertions;
using Xunit;

namespace CShopa.Tests.OpaPolicyTests;

public class EvaluateShould : OpaPolicyTestBase
{
    public EvaluateShould(RuntimeFixture fixture)
        : base(fixture)
    {
    }

    [Theory]
    [InlineData(Runtime.Wasmtime)]
    [InlineData(Runtime.Wasmer)]
    public void ReturnSimpleEvaluateWithData(Runtime runtime)
    {
        var policy = ExamplePolicy(runtime);
        policy.SetData(new { world = "hello" });

        var result = policy.Evaluate<bool>(new { message = "hello" });

        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(Runtime.Wasmtime)]
    [InlineData(Runtime.Wasmer)]
    public void HonorDefaultResponse(Runtime runtime)
    {
        var policy = ExamplePolicy(runtime);
        var result = policy.Evaluate<bool>();
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(Runtime.Wasmtime)]
    [InlineData(Runtime.Wasmer)]
    public void ReturnJson(Runtime runtime)
    {
        var policy = ExamplePolicy(runtime);
        policy.SetData(new { world = "hello" });

        var result = policy.Evaluate<bool>(new { message = "hello" }, out var json);

        result.Should().BeTrue();
        json.Should().Be(@"[{""result"":true}]");
    }
}
