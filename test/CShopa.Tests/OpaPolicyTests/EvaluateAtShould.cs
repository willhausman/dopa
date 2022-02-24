using FluentAssertions;
using Xunit;

namespace CShopa.Tests.OpaPolicyTests;

public class EvaluateAtShould : OpaPolicyTestBase
{
    public EvaluateAtShould(RuntimeFixture fixture)
        : base(fixture)
    {
    }

    [Theory]
    [InlineData(Runtime.Wasmtime)]
    [InlineData(Runtime.Wasmer)]
    public void ReturnValueForEntrypoint(Runtime runtime)
    {
        using var policy = BuiltinsPolicy(runtime);
        policy.AddBuiltin("custom.builtin0", () => 0);
        policy.AddBuiltin("custom.builtin1", (int i) => 0);
        policy.AddBuiltin("custom.builtin2", (int i, int j) => 0);
        policy.AddBuiltin("custom.builtin3", (int i, int j, int k) => 0);
        policy.AddBuiltin("custom.builtin4", (int i, int j, int k, int l) => 0);

        policy.Entrypoints.Should().Contain("builtins/sdkBuiltinValue");
        var result = policy.EvaluateAt<string>("builtins/sdkBuiltinValue") ?? throw new NullReferenceException();
        result.Should().Be("number");
    }
}
