using Opa.WebAssembly.Wasmtime;

namespace Opa.WebAssembly.Tests;

public class WasmtimePolicyFixture
{
    public IOpaModule ExampleModule { get; } = WasmModule.FromFile("policies/example.wasm");
}
