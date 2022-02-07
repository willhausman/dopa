using Xunit;

namespace CShopa.Tests
{
    [CollectionDefinition(nameof(WasmtimePolicyCollection))]
    public class WasmtimePolicyCollection : ICollectionFixture<WasmtimePolicyFixture>
    {
    }
}
