using Xunit;
using DOPA.Loader;
using FluentAssertions;

namespace DOPA.Tests.OpaLoaderTests;

public class CreateWebAssemblyStreamShould
{
    [Fact]
    public async Task ReturnSimpleModule()
    {
        using var stream = await OpaLoader.CreateWebAssemblyStream("policies/example.rego", "example/hello");
        using var module = WasmModule.FromStream("example", stream);
        using var policy = module.CreatePolicy();
        policy.SetData(new { world = "hello" });

        var result = policy.Evaluate<bool>(new { message = "hello" });
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ReturnModuleWithCapabilities()
    {
        var args = new OpaArguments("policies/builtins.rego", "builtins")
        {
            Capabilities = "policies/builtins.capabilities.json"
        };
        using var stream = await OpaLoader.CreateWebAssemblyStream(args);
        using var module = WasmModule.FromStream("builtins", stream);
        using var policy = module.CreatePolicy();
        policy.AddBuiltin("custom.builtin0", () => 1);
        policy.AddBuiltin("custom.builtin1", (int i) => 2);
        policy.AddBuiltin("custom.builtin2", (int i, int j) => 3);
        policy.AddBuiltin("custom.builtin3", (int i, int j, int k) => 4);
        policy.AddBuiltin("custom.builtin4", (int i, int j, int k, int l) => 5);

        var result = policy.EvaluateAt<string>("builtins/sdkBuiltinValue");
        result.Should().Be("number");
    }
}
