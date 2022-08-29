using DOPA.Exceptions;
using Wasmtime;

namespace DOPA.Runtime
{
    /// <summary>
    /// Convenience extensions for configuring the Wasmtime <see cref="Linker" />.
    /// </summary>
    public static class LinkerExtensions
    {
        /// <summary>
        /// Link necessary imports and builtins for working with OPA.
        /// </summary>
        /// <param name="linker">A <see cref="Linker"/> instance.</param>
        /// <param name="store">A <see cref="Store" /> instance.</param>
        /// <param name="memory">The shared <see cref="Memory" /> instance.</param>
        /// <param name="builtins">The delegates to call when certain builtins are called.</param>
        /// <returns></returns>
        public static Linker LinkForOpa(this Linker linker, Store store, Memory memory, IBuiltinCollection builtins) =>
            linker
                .DefineGlobalImports(store, memory)
                .DefineBuiltins(store, builtins);

        private static Linker DefineGlobalImports(this Linker linker, Store store, Memory memory)
        {
            linker.Define(WellKnown.Imports.Namespace, WellKnown.Imports.memory, memory);

            linker.Define(
                WellKnown.Imports.Namespace,
                WellKnown.Imports.opa_abort,
                Function.FromCallback(
                    store,
                    (Caller caller, int address) =>
                        throw OpaAbortException.Because(memory.ReadNullTerminatedString(address))));
            return linker;
        }

        private static Linker DefineBuiltins(this Linker linker, Store store, IBuiltinCollection builtins)
        {
            linker.Define(WellKnown.Imports.Namespace, WellKnown.Imports.opa_builtin0, Function.FromCallback(store, (Caller caller, int builtinId, int _) => builtins[builtinId].Invoke()));
            linker.Define(WellKnown.Imports.Namespace, WellKnown.Imports.opa_builtin1, Function.FromCallback(store, (Caller caller, int builtinId, int _, int address1) => builtins[builtinId].Invoke(address1)));
            linker.Define(WellKnown.Imports.Namespace, WellKnown.Imports.opa_builtin2, Function.FromCallback(store, (Caller caller, int builtinId, int _, int address1, int address2) => builtins[builtinId].Invoke(address1, address2)));
            linker.Define(WellKnown.Imports.Namespace, WellKnown.Imports.opa_builtin3, Function.FromCallback(store, (Caller caller, int builtinId, int _, int address1, int address2, int address3) => builtins[builtinId].Invoke(address1, address2, address3)));
            linker.Define(WellKnown.Imports.Namespace, WellKnown.Imports.opa_builtin4, Function.FromCallback(store, (Caller caller, int builtinId, int _, int address1, int address2, int address3, int address4) => builtins[builtinId].Invoke(address1, address2, address3, address4)));

            return linker;
        }
    }
}
