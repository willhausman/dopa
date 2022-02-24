using BenchmarkDotNet.Attributes;
using CShopa.Runtime;

namespace CShopa.Benchmarks;

[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
public class CreatePolicyBenchmark
{
    private IOpaModule module;
    private IOpaPolicy policy;

    [GlobalSetup]
    public void Setup()
    {
        module = WasmModule.FromFile("policies/example.wasm");
    }

    [GlobalCleanup]
    public void Cleanup() => module.Dispose();

    [IterationCleanup]
    public void PolicyCleanup() => policy.Dispose();

    [Benchmark]
    public void CreatePolicy() =>
        policy = module.CreatePolicy();
}
