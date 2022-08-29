using DOPA.Cli;
using FluentAssertions;
using Xunit;

namespace DOPA.Tests.OpaPolicyTests;

public class AddBuiltinShould : OpaPolicyTestBase
{
    public AddBuiltinShould(RuntimeFixture fixture)
        : base(fixture)
    {
    }

    [Theory]
    [InlineData(Runtime.Wasmtime)]
    public void ThrowWhenBuiltinUndefined(Runtime runtime)
    {
        using var policy = BuiltinsPolicy(runtime);

        Action act = () => policy.Evaluate<object>();

        act.Should().Throw<Exception>().WithMessage("*Builtin not defined*");
    }

    [Theory]
    [InlineData(Runtime.Wasmtime)]
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

    [Fact]
    public void CallNestedPolicies()
    {
        using var bundle1 = Opa.Build
            .WebAssembly()
            .Files("policies/builtins.rego")
            .Entrypoints("builtins/secondValue")
            .Capabilities("policies/builtins.capabilities.json")
            .Execute();
        using var stream1 = bundle1.ExtractWebAssemblyModule();
        using var module1 = WasmModule.FromStream("stream1", stream1);
        using var policy1 = module1.CreatePolicy();
        policy1.AddBuiltin("custom.builtin0", () => 1);
        policy1.AddBuiltin("custom.builtin1", (int i) => i + 1);

        using var bundle2 = Opa.Build
            .WebAssembly()
            .Files("policies/builtins.rego")
            .Entrypoints("builtins/firstValue")
            .Capabilities("policies/builtins.capabilities.json")
            .Execute();
        using var stream2 = bundle2.ExtractWebAssemblyModule();
        using var module2 = WasmModule.FromStream("stream2", stream2);
        using var policy2 = module2.CreatePolicy();
        policy2.AddBuiltin("custom.builtin0", () => policy1.Evaluate<int>() + 1);

        var result = policy2.Evaluate<int>();
        result.Should().Be(3);
    }

    private record BuiltinsResult(int FirstValue, string SecondValue, string[] ThirdValue, bool FourthValue, Builtin4Result FifthValue, string SdkBuiltinValue);
    private record Builtin4Result(int FirstValue, string SecondValue, string[] ThirdValue, bool FourthValue);
}
