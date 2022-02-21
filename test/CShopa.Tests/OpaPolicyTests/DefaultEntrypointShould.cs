using FluentAssertions;
using Xunit;

namespace CShopa.Tests.OpaPolicyTests;

public class DefaultEntrypointShould : OpaPolicyTestBase
{
    public DefaultEntrypointShould(RuntimeFixture fixture)
        : base(fixture)
    {
    }

    [Theory]
    [InlineData(Runtime.Wasmtime)]
    [InlineData(Runtime.Wasmer)]
    public void ReturnValueForChangedDefault(Runtime runtime)
    {
        var policy = BuiltinsPolicy(runtime);
        policy.AddBuiltin("custom.builtin0", () => 0);
        policy.AddBuiltin("custom.builtin1", (int i) => 0);
        policy.AddBuiltin("custom.builtin2", (int i, int j) => 0);
        policy.AddBuiltin("custom.builtin3", (int i, int j, int k) => 0);
        policy.AddBuiltin("custom.builtin4", (int i, int j, int k, int l) => 0);
        policy.DefaultEntrypoint = "builtins/sdkbuiltinvalue";

        var result = policy.Evaluate<string>() ?? throw new NullReferenceException();
        result.Should().Be("number");
    }

    [Theory]
    [InlineData(Runtime.Wasmtime)]
    [InlineData(Runtime.Wasmer)]
    public void ThrowWhenSettingUnknownEntrypoint(Runtime runtime)
    {
        var policy = ExamplePolicy(runtime);
        var unexpected = "something-crazy";

        Action act = () => policy.DefaultEntrypoint = unexpected;

        act.Should().Throw<ArgumentException>();
    }
}
