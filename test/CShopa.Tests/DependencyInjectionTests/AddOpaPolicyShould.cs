using CShopa.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CShopa.Tests.DependencyInjectionTests;

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
}
