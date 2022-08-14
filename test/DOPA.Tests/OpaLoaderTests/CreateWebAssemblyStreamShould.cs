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
}
