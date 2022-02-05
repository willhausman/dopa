using System.Diagnostics.CodeAnalysis;
using Wasmtime;

namespace Opa.WebAssembly.Wasmtime;

internal sealed class WasmtimeInstance : Disposable, IWasmInstance
{
    [NotNull]
    private Instance? instance;
    private readonly CallbackCollection callbacks;
    private readonly PolicyRuntime runtime;
    private Store store => runtime.Store;
    private Linker linker => runtime.Linker;

    public WasmtimeInstance(PolicyRuntime runtime, Module module)
    {
        this.runtime = runtime;
        this.callbacks = new CallbackCollection(runtime, new Callback[0]);

        this.instance = linker.Instantiate(store, module);
    }

    ~WasmtimeInstance()
    {
        Dispose(false);
    }

    protected override void DisposeManaged()
    {
        this.runtime.Dispose();
    }

    protected override void DisposeUnmanaged()
    {
        this.instance = null;
    }
}
