using Wasmtime;

namespace Opa.WebAssembly.Wasmtime;

using System.Diagnostics.CodeAnalysis;
using Exceptions;

internal class PolicyRuntime : Disposable
{
    public PolicyRuntime(Engine engine)
    {
        Store = new Store(engine);
        Linker = new Linker(engine);
        Memory = new Memory(Store, 2);

        LinkGlobalImports();
    }

    public Store Store { get; }

    public Linker Linker { get; }

    [NotNull]
    public Memory? Memory { get; private set; }

    private void LinkGlobalImports()
    {
        Define(WellKnown.Imports.memory, Memory);

        Define(
            WellKnown.Imports.opa_abort,
            Function.FromCallback(
                Store,
                (Caller caller, int address) =>
                    throw OpaAbortException.Because(Memory.ReadNullTerminatedString(Store, address))));
    }

    private void Define(string name, object item) => Linker.Define(WellKnown.Imports.Namespace, name, item);

    protected override void DisposeManaged()
    {
        Store.Dispose();
        Linker.Dispose();
    }

    protected override void DisposeUnmanaged()
    {
        Memory = null;
    }
}
