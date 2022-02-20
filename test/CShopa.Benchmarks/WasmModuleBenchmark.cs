using BenchmarkDotNet.Attributes;
using CShopa.Wasmtime;

namespace CShopa.Benchmarks;

[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
public class WasmModuleBenchmark
{
    private IOpaModule module;

    [IterationCleanup]
    public void Cleanup() => module.Dispose();

    [Benchmark]
    public void CreateModule() =>
        module = WasmModule.FromFile("policies/example.wasm");
}
