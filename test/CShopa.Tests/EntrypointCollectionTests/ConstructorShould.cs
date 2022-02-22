using FluentAssertions;
using Xunit;

namespace CShopa.Tests.EntrypointCollectionTests;

public class ConstructorShould
{
    [Fact]
    public void ThrowIfNoDefaultValue()
    {
        Action act = () => new EntrypointCollection(new Dictionary<string, int>());

        act.Should().Throw<ArgumentException>();
    }
}
