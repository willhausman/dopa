using BenchmarkDotNet.Attributes;
using DOPA.Runtime;

namespace DOPA.Benchmarks;

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
