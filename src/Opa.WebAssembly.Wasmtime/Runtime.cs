using Wasmtime;

namespace Opa.WebAssembly.Wasmtime;

using Exceptions;

internal class Runtime : IDisposable
{
    private bool disposedValue;

    public Runtime(Engine engine)
    {
        Store = new Store(engine);
        Linker = new Linker(engine);
        Memory = new Memory(Store, 2);
    }

    public Store Store { get; }

    public Linker Linker { get; }

    public Memory Memory { get; }

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

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Store.Dispose();
                Linker.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
