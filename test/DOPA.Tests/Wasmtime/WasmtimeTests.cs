using System;
using FluentAssertions;
using Wasmtime;
using Xunit;

namespace DOPA.Tests.Wasmtime;

/// <summary>
/// Collection of tests used to verify the approach taken for the Wasmtime implementation of <see cref="IOpaRuntime" />
/// </summary>
public class WasmtimeTests
{
    [Fact]
    public void InstantiationThrowsWhenImportsNotLinkedYet()
    {
        using var engine = new Engine();
        using var module = Module.FromText(engine, "hello", SampleWat);

        using var linker = new Linker(engine);
        using var store = new Store(engine);

        Action act = () => linker.Instantiate(store, module);

        act.Should().Throw<WasmtimeException>();
    }

    [Fact]
    public void InstanceRunsWhenImportsLinked()
    {
        var expected = 1;
        using var engine = new Engine();
        using var module = Module.FromText(engine, "hello", SampleWat);

        using var linker = new Linker(engine);
        using var store = new Store(engine);

        linker.Define(
                "",
                "hello",
                Function.FromCallback(store, () => expected)
            );

        var instance = linker.Instantiate(store, module);

        var result = instance.GetFunction(store, "run")?.Invoke(store);
        result.Should().Be(expected);
    }

    [Fact]
    public void InstanceRunsAfterLinkerDisposed()
    {
        var expected = 1;
        using var engine = new Engine();
        using var module = Module.FromText(engine, "hello", SampleWat);
        using var store = new Store(engine);

        Instance instance;

        using (var linker = new Linker(engine))
        {
            linker.Define(
                "",
                "hello",
                Function.FromCallback(store, () => expected)
            );

            instance = linker.Instantiate(store, module);
        }

        var result = instance.GetFunction(store, "run")?.Invoke(store);
        result.Should().Be(expected);
    }

    private const string SampleWat = @"(module
  (import """" ""hello"" (func $.hello (result i32)))
  (func (export ""run"") (result i32) call $.hello))";
}
