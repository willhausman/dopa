using Xunit;
using DOPA.Loader;
using FluentAssertions;

namespace DOPA.Tests.OpaLoaderTests;

public class CreateWebAssemblyModuleAsyncShould
{
    [Fact]
    public async Task ReturnSimpleModule()
    {
        using var stream = await OpaLoader.CreateWebAssemblyModuleAsync("policies/example.rego", "example/hello");
        using var module = WasmModule.FromStream("example", stream);
        using var policy = module.CreatePolicy();
        policy.SetData(new { world = "hello" });

        var result = policy.Evaluate<bool>(new { message = "hello" });
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ReturnModuleWithCapabilities()
    {
        var args = new OpaArguments("policies/builtins.rego", "builtins/firstValue")
        {
            Capabilities = "policies/builtins.capabilities.json"
        };
        using var stream = await OpaLoader.CreateWebAssemblyModuleAsync(args);
        using var module = WasmModule.FromStream("builtins", stream);
        using var policy = module.CreatePolicy();
        policy.AddBuiltin("custom.builtin0", () => 1);

        var result = policy.Evaluate<int>();
        result.Should().Be(1);
    }
}
