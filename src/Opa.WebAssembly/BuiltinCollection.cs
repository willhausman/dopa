using System.Collections.Concurrent;

namespace Opa.WebAssembly;

public class BuiltinCollection
{
    private IDictionary<string, int> builtinIds;
    private readonly ConcurrentDictionary<int, Builtin> builtins = new();

    public BuiltinCollection(IDictionary<int, string> builtinMap)
    {
        this.builtinIds = builtinMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
    }

    public void AddBuiltin(Builtin callback)
    {
        if (builtinIds.TryGetValue(callback.Name, out var address))
        {
            builtins.AddOrUpdate(address, _ => callback, (_, oldCallback) => callback);
            return;
        }

        throw new InvalidOperationException("Cannot add builtin.");
    }

    public Builtin this[int address]
    {
        get
        {
            if (builtins.TryGetValue(address, out var builtin))
            {
                return builtin;
            }

            throw new InvalidOperationException("Builtin not defined.");
        }
    }
}
