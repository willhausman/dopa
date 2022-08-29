using DOPA.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DOPA.Tests.DependencyInjectionTests;

public class AddOpaPolicyShould
{
    private readonly string examplePath = "policies/example.wasm";
    [Fact]
    public void DisposeTheModule()
    {
        var provider = new ServiceCollection()
            .AddOpaPolicy(examplePath)
            .BuildServiceProvider();

        var module = provider.GetRequiredService<IOpaModule>();

        provider.Dispose();

        module.Disposed.Should().BeTrue();
    }

    [Fact]
    public void ReturnAUsableInstanceFromAFile()
    {
        using var provider = new ServiceCollection()
            .AddOpaPolicy(examplePath)
            .BuildServiceProvider();
        
        using var policy = provider.GetRequiredService<IOpaPolicy>();
        InstanceShouldBeUsable(policy);
    }

    [Fact]
    public void ReturnAUsableInstanceFromBytes()
    {
        var contents = File.ReadAllBytes(examplePath);
        using var provider = new ServiceCollection()
            .AddOpaPolicy("policy", contents)
            .BuildServiceProvider();
        
        using var policy = provider.GetRequiredService<IOpaPolicy>();
        InstanceShouldBeUsable(policy);
    }

    [Fact]
    public void ReturnAUsableInstanceFromStream()
    {
        using var stream = File.OpenRead(examplePath);
        using var provider = new ServiceCollection()
            .AddOpaPolicy("policy", stream)
            .BuildServiceProvider();
        
        using var policy = provider.GetRequiredService<IOpaPolicy>();
        InstanceShouldBeUsable(policy);
    }

    [Fact]
    public void ReturnTypedInstancesFromStream()
    {
        using var stream1 = File.OpenRead(examplePath);
        using var stream2 = File.OpenRead(examplePath);
        using var provider = new ServiceCollection()
            .AddOpaPolicy<Type1>(stream1)
            .AddOpaPolicy<Type2>(stream2)
            .BuildServiceProvider();
        
        using var policy1 = provider.GetRequiredService<IOpaPolicy<Type1>>();
        using var policy2 = provider.GetRequiredService<IOpaPolicy<Type2>>();
        InstanceShouldBeUsable(policy1);
        InstanceShouldBeUsable(policy2);
    }

    [Fact]
    public void ReturnTypedInstancesFromFile()
    {
        using var provider = new ServiceCollection()
            .AddOpaPolicy<Type1>(examplePath)
            .AddOpaPolicy<Type2>(examplePath)
            .BuildServiceProvider();
        
        using var policy1 = provider.GetRequiredService<IOpaPolicy<Type1>>();
        using var policy2 = provider.GetRequiredService<IOpaPolicy<Type2>>();
        InstanceShouldBeUsable(policy1);
        InstanceShouldBeUsable(policy2);
    }

    [Fact]
    public void ReturnTypedInstancesFromBytes()
    {
        var contents1 = File.ReadAllBytes(examplePath);
        var contents2 = File.ReadAllBytes(examplePath);
        using var provider = new ServiceCollection()
            .AddOpaPolicy<Type1>(contents1)
            .AddOpaPolicy<Type2>(contents1)
            .BuildServiceProvider();
        
        using var policy1 = provider.GetRequiredService<IOpaPolicy<Type1>>();
        using var policy2 = provider.GetRequiredService<IOpaPolicy<Type2>>();
        InstanceShouldBeUsable(policy1);
        InstanceShouldBeUsable(policy2);
    }

    private record Type1();
    private record Type2();

    private void InstanceShouldBeUsable(IOpaPolicy policy)
    {
        policy.Should().NotBeNull();
        policy.SetData(new { world = "hello" });
        policy.Evaluate<bool>(new { message = "hello" }).Should().BeTrue();
    }
}
