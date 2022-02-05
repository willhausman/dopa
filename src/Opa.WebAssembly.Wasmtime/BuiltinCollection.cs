using Wasmtime;

namespace Opa.WebAssembly.Wasmtime;

internal class BuiltinCollection
{
    private Runtime runtime;
    private IStore Store => runtime.Store;
    private Linker Linker => runtime.Linker;

	  public BuiltinCollection(Runtime runtime)
    {
        this.runtime = runtime;
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
