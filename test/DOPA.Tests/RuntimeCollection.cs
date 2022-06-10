using Xunit;

namespace DOPA.Tests
{
    [CollectionDefinition(nameof(RuntimeCollection))]
    public class RuntimeCollection : ICollectionFixture<RuntimeFixture>
    {
    }
}
