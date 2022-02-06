using System.Collections.Concurrent;
using Opa.WebAssembly.Extensions;

namespace Opa.WebAssembly;

public class BuiltinCollection : IBuiltinCollection
{
    private IDictionary<string, int> builtinIds;
    private readonly ConcurrentDictionary<int, Builtin> builtins = new();

    public BuiltinCollection()
    {
        this.builtinIds = new Dictionary<string, int>();
    }

    public BuiltinCollection(IDictionary<int, string> builtinMap)
    {
        this.builtinIds = builtinMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
    }

    public IDictionary<int, string> BuiltinMap
    {
        get
        {
            return this.builtinIds.ToReverseDictionary();
        }
        set
        {
            if (this.builtinIds.Any())
            {
                throw new InvalidOperationException("Builtin collection cannot be reinitialized.");
            }

            this.builtinIds = value.ToReverseDictionary();
        }
    }

    public bool AddBuiltin(Builtin callback)
    {
        if (builtinIds.TryGetValue(callback.Name, out var address))
        {
            builtins.AddOrUpdate(address, _ => callback, (_, oldCallback) => callback);
            return true;
        }
        
        return false;
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
