using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Opa.WebAssembly.Tests.OpaPolicyTests;

[Collection(nameof(WasmtimePolicyCollection))]
public class EvaluateShould
{
    private readonly WasmtimePolicyFixture fixture;

    public EvaluateShould(WasmtimePolicyFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void ReturnSimpleEvaluateWithData()
    {
        // Given
        var policy = fixture.ExampleModule.CreatePolicy();
        policy.SetData(new { world = "hello" });
    
        // When
        var result = policy.Evaluate<IEnumerable<OpaResult<bool>>>(new { message = "hello" });
    
        // Then
        result?.FirstOrDefault()?.Result.Should().BeTrue();
    }

    private record OpaResult<T>(T Result);
}
