using DOPA.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DOPA.Tests.DependencyInjectionTests;

public class AddOpaPolicyShould
{
    [Fact]
    public void DisposeTheModule()
    {
        var provider = new ServiceCollection()
            .AddOpaPolicy("policies/example.wasm")
            .BuildServiceProvider();

        var module = provider.GetRequiredService<IOpaModule>();

        provider.Dispose();

        module.Disposed.Should().BeTrue();
    }

    [Fact]
    public void ReturnAUsableInstanceFromAFile()
    {
        using var provider = new ServiceCollection()
            .AddOpaPolicy("policies/example.wasm")
            .BuildServiceProvider();
        
        using var policy = provider.GetRequiredService<IOpaPolicy>();
        InstanceShouldBeUsable(policy);
    }

    [Fact]
    public void ReturnAUsableInstanceFromBytes()
    {
        var contents = File.ReadAllBytes("policies/example.wasm");
        using var provider = new ServiceCollection()
            .AddOpaPolicy("policy", contents)
            .BuildServiceProvider();
        
        using var policy = provider.GetRequiredService<IOpaPolicy>();
        InstanceShouldBeUsable(policy);
    }

    [Fact]
    public void ReturnAUsableInstanceFromStream()
    {
        var stream = File.OpenRead("policies/example.wasm");
        using var provider = new ServiceCollection()
            .AddOpaPolicy("policy", stream)
            .BuildServiceProvider();
        
        using var policy = provider.GetRequiredService<IOpaPolicy>();
        InstanceShouldBeUsable(policy);
    }

    private void InstanceShouldBeUsable(IOpaPolicy policy)
    {
        policy.Should().NotBeNull();
        policy.SetData(new { world = "hello" });
        policy.Evaluate<bool>(new { message = "hello" }).Should().BeTrue();
    }
}
