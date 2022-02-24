using FluentAssertions;
using Xunit;

namespace CShopa.Tests.OpaPolicyTests;

public class AddBuiltinShould : OpaPolicyTestBase
{
    public AddBuiltinShould(RuntimeFixture fixture)
        : base(fixture)
    {
    }

    [Theory]
    [InlineData(Runtime.Wasmtime)]
    // [InlineData(Runtime.Wasmer)] interestingly, this test causes the test explorer to crash on Mac. Wasmer feeling worthless as a runtime.
    public void ThrowWhenBuiltinUndefined(Runtime runtime)
    {
        using var policy = BuiltinsPolicy(runtime);

        Action act = () => policy.Evaluate<object>();

        // neither runtime does this in a very friendly way, but they do both throw as expected
        act.Should().Throw<Exception>();
    }

    [Theory]
    [InlineData(Runtime.Wasmtime)]
    [InlineData(Runtime.Wasmer)]
    public void CallAddedBuiltins(Runtime runtime)
    {
        using var policy = BuiltinsPolicy(runtime);
        policy.AddBuiltin("custom.builtin0", () => 1);
        policy.AddBuiltin("custom.builtin1", (int i) => "2");
        policy.AddBuiltin("custom.builtin2", (int i, string j) => new [] { i.ToString(), j });
        policy.AddBuiltin("custom.builtin3", (int i, string j, string[] k) => i == 1 && j == "2" && k[0] == i.ToString() && k[1] == j);
        policy.AddBuiltin("custom.builtin4", (int i, string j, string[] k, bool matches) => new Builtin4Result(i, j, k, matches));

        var result = policy.Evaluate<BuiltinsResult>() ?? throw new NullReferenceException();

        result.FirstValue.Should().Be(1);
        result.SecondValue.Should().Be("2");
        result.ThirdValue.Should().BeEquivalentTo(new [] { "1", "2" });
        result.FourthValue.Should().BeTrue();
        result.FifthValue.Should().BeEquivalentTo(new Builtin4Result(1, "2", new [] { "1", "2" }, true));
        result.SdkBuiltinValue.Should().Be("object");
    }

    private record BuiltinsResult(int FirstValue, string SecondValue, string[] ThirdValue, bool FourthValue, Builtin4Result FifthValue, string SdkBuiltinValue);
    private record Builtin4Result(int FirstValue, string SecondValue, string[] ThirdValue, bool FourthValue);
}
