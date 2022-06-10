using FluentAssertions;
using Xunit;

namespace DOPA.Tests.EntrypointCollectionTests;

public class DefaultEntrypointShould
{
    private readonly Dictionary<string, int> entrypoints = new()
    {
        { "default", WellKnown.Values.DefaultEntrypointId },
        { "backup", 1 },
    };

    [Fact]
    public void MatchWellKnownValueByDefault()
    {
        var collection = new EntrypointCollection(entrypoints);

        collection.DefaultEntrypoint.Should().Be("default");
    }

    [Fact]
    public void UpdateToNewValue()
    {
        var collection = new EntrypointCollection(entrypoints);
        collection.DefaultEntrypoint = "backup";

        collection.DefaultEntrypoint.Should().Be("backup");
    }

    [Fact]
    public void ThrowWhenUpdatingWithUnknownEntrypoint()
    {
        var collection = new EntrypointCollection(entrypoints);

        Action act = () => collection.DefaultEntrypoint = "invalid-entrypoint";
        act.Should().Throw<ArgumentException>();
    }
}
