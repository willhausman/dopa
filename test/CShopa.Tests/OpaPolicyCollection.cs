using Xunit;

namespace CShopa.Tests
{
    [CollectionDefinition(nameof(OpaPolicyCollection))]
    public class OpaPolicyCollection : ICollectionFixture<RuntimeFixture>
    {
    }
}
