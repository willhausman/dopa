using CShopa.Benchmarks;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<EvaluateOverflowBenchmark>();
BenchmarkRunner.Run<CreatePolicyBenchmark>();
BenchmarkRunner.Run<WasmModuleBenchmark>();
