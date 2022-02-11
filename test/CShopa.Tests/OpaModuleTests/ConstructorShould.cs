using FluentAssertions;
using Moq;
using Xunit;

namespace CShopa.Tests.OpaModuleTests;

public class ConstructorShould : OpaModuleTestBase
{
    public ConstructorShould(RuntimeFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public void ValidateSupportedExports()
    {
        var module = new Mock<IWasmModule>();
        module.Setup(r => r.Exports).Returns(new [] { "not_opa_eval" });

        Action act = () => new OpaModule(module.Object);

        act.Should().Throw<ArgumentException>().WithParameterName("module");
    }
}
