using Xunit;

namespace CShopa.Tests
{
    [CollectionDefinition(nameof(RuntimeCollection))]
    public class RuntimeCollection : ICollectionFixture<RuntimeFixture>
    {
    }
}
