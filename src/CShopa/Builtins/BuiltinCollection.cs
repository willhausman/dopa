using System.Collections.Concurrent;

namespace CShopa.Builtins;

public class BuiltinCollection : IBuiltinCollection
{
    private IDictionary<string, int> builtinIds;
    private readonly ConcurrentDictionary<int, IBuiltin> builtins = new();

    public BuiltinCollection()
    {
        this.builtinIds = new Dictionary<string, int>();
    }

    public bool ConfigureBuiltinIds(IDictionary<string, int> builtinIds)
    {
        if (this.builtinIds.Any())
        {
            return false;
        }
        this.builtinIds = builtinIds;
        return true;
    }

    public IReadOnlyCollection<IBuiltin> Builtins => this.builtins.Values.ToList();

    public bool AddBuiltin(IBuiltin callback)
    {
        if (builtinIds.TryGetValue(callback.Name, out var address))
        {
            builtins.AddOrUpdate(address, _ => callback, (_, oldCallback) => callback);
            return true;
        }
        
        return false;
    }

    public IBuiltin this[int builtinId]
    {
        get
        {
            if (builtins.TryGetValue(builtinId, out var builtin))
            {
                return builtin;
            }

            throw new InvalidOperationException("Builtin not defined.");
        }
    }
}
