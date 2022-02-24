using System.Text.Json;
using BenchmarkDotNet.Attributes;
using CShopa.Runtime;

namespace CShopa.Benchmarks;

[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
public class EvaluateOverflowBenchmark
{
    private IOpaPolicy policy;
    private object input;
    private string inputJson;

    [Params(1, 10, 100, 1000)]
    public int messageSizeKb;

    [IterationSetup]
    public void Setup()
    {
        policy = WasmModule.FromFile("policies/example.wasm").CreatePolicy();
        policy.SetData(new { world = "hello" });
        var growingMessage = new string('A', messageSizeKb * 1024);
        input = new { growingMessage };
        inputJson = JsonSerializer.Serialize(input, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }

    [IterationCleanup]
    public void Cleanup() => policy.Dispose();

    [Benchmark]
    public string EvaluateJson() => policy.EvaluateJson(inputJson);

    [Benchmark]
    public bool Evaluate() => policy.Evaluate<bool>(input);
}
