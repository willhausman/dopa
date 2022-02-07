using CShopa.Wasmtime;

namespace CShopa.Tests;

public class WasmtimePolicyFixture
{
    public IOpaModule ExampleModule { get; } = WasmModule.FromFile("policies/example.wasm");
}
