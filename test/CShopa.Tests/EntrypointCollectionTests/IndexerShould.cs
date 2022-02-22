using FluentAssertions;
using Xunit;

namespace CShopa.Tests.EntrypointCollectionTests;

public class IndexerShould
{
    private readonly Dictionary<string, int> entrypoints = new()
    {
        { "default", WellKnown.Values.DefaultEntrypointId },
        { "backup", 1 },
    };

    [Fact]
    public void ReturnEntrypointId()
    {
        var collection = new EntrypointCollection(entrypoints);

        collection["backup"].Should().Be(1);
    }

    [Fact]
    public void ReturnDefaultEntrypointId()
    {
        var collection = new EntrypointCollection(entrypoints);

        collection["invalid-entrypoint"].Should().Be(WellKnown.Values.DefaultEntrypointId);
    }
}
