using Xunit;

namespace Opa.WebAssembly.Tests
{
    [CollectionDefinition(nameof(WasmtimePolicyCollection))]
    public class WasmtimePolicyCollection : ICollectionFixture<WasmtimePolicyFixture>
    {
    }
}
