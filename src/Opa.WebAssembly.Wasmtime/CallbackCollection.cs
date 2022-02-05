using System.Collections.Concurrent;
using Opa.WebAssembly.Extensions;
using Wasmtime;

namespace Opa.WebAssembly.Wasmtime;

internal class CallbackCollection
{
    private IDictionary<int, string> builtins = new Dictionary<int, string>();
    private readonly ConcurrentDictionary<string, Callback> callbacks = new();
    private PolicyRuntime runtime;
    private IStore Store => runtime.Store;
    private Linker Linker => runtime.Linker;

    public CallbackCollection(PolicyRuntime runtime, IEnumerable<Callback> callbacks)
    {
        this.runtime = runtime;
        this.callbacks = callbacks.ToConcurrentDictionary(c => c.Name);

        LinkBuiltins();
    }

    private void LinkBuiltins()
    {
        Define(WellKnown.Imports.opa_builtin0, Function.FromCallback(Store, (Caller caller, int builtinId, int _) => { }));
        Define(WellKnown.Imports.opa_builtin1, Function.FromCallback(Store, (Caller caller, int builtinId, int _, int address1) => { }));
        Define(WellKnown.Imports.opa_builtin2, Function.FromCallback(Store, (Caller caller, int builtinId, int _, int address1, int address2) => { }));
        Define(WellKnown.Imports.opa_builtin3, Function.FromCallback(Store, (Caller caller, int builtinId, int _, int address1, int address2, int address3) => { }));
        Define(WellKnown.Imports.opa_builtin4, Function.FromCallback(Store, (Caller caller, int builtinId, int _, int address1, int address2, int address3, int address4) => { }));
    }

    private void Define(string name, object item) => Linker.Define(WellKnown.Imports.Namespace, name, item);
}
