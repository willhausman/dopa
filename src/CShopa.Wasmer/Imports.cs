using CShopa.Exceptions;
using WasmerSharp;

namespace CShopa.Wasmer;

public static class Imports
{
    public static Import[] ForOpa(Memory memory, IBuiltinCollection collection) =>
        Globals(memory).Concat(Builtins(collection)).ToArray();

    private static Import[] Globals(Memory memory)
    {
        OpaAbort opaAbort = (InstanceContext ctx, int address) =>
            throw OpaAbortException.Because(memory.ReadNullTerminatedString(address));

        return new Import[]
        {
            new(
                WellKnown.Imports.Namespace,
                WellKnown.Imports.memory,
                memory),

            new(
                WellKnown.Imports.Namespace,
                WellKnown.Imports.opa_abort,
                new ImportFunction(opaAbort))
        };
    }

    private static Import[] Builtins(IBuiltinCollection collection)
    {
        Builtin0 builtin0 = (InstanceContext ctx, int builtinId, int _) => collection[builtinId].Invoke();
        Builtin1 builtin1 = (InstanceContext ctx, int builtinId, int _, int address) => collection[builtinId].Invoke(address);
        Builtin2 builtin2 = (InstanceContext ctx, int builtinId, int _, int address1, int address2) => collection[builtinId].Invoke(address1, address2);
        Builtin3 builtin3 = (InstanceContext ctx, int builtinId, int _, int address1, int address2, int address3) => collection[builtinId].Invoke(address1, address2, address3);
        Builtin4 builtin4 = (InstanceContext ctx, int builtinId, int _, int address1, int address2, int address3, int address4) => collection[builtinId].Invoke(address1, address2, address3, address4);

        return new Import[]
        {
            new(
                WellKnown.Imports.Namespace,
                WellKnown.Imports.opa_builtin0,
                new ImportFunction(builtin0)),
            new(
                WellKnown.Imports.Namespace,
                WellKnown.Imports.opa_builtin1,
                new ImportFunction(builtin1)),
            new(
                WellKnown.Imports.Namespace,
                WellKnown.Imports.opa_builtin2,
                new ImportFunction(builtin2)),
            new(
                WellKnown.Imports.Namespace,
                WellKnown.Imports.opa_builtin3,
                new ImportFunction(builtin3)),
            new(
                WellKnown.Imports.Namespace,
                WellKnown.Imports.opa_builtin4,
                new ImportFunction(builtin4)),
        };
    }

    delegate void OpaAbort(InstanceContext ctx, int address);
    delegate int Builtin0(InstanceContext ctx, int builtinId, int _);
    delegate int Builtin1(InstanceContext ctx, int builtinId, int _, int address);
    delegate int Builtin2(InstanceContext ctx, int builtinId, int _, int address1, int address2);
    delegate int Builtin3(InstanceContext ctx, int builtinId, int _, int address1, int address2, int address3);
    delegate int Builtin4(InstanceContext ctx, int builtinId, int _, int address1, int address2, int address3, int address4);
}
