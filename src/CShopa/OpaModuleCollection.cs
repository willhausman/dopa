namespace CShopa;

public class OpaModuleCollection : IOpaModuleCollection
{
    private readonly IDictionary<string, IOpaModule> modules;

    public OpaModuleCollection(IEnumerable<IOpaModule> modules)
    {
        this.modules = modules.ToDictionary(m => m.Name, StringComparer.OrdinalIgnoreCase);
    }

    public IOpaModule this[string name]
    {
        get
        {
            if (modules.ContainsKey(name))
            {
                return modules[name];
            }
            
            throw new ArgumentException($"No module has been registered with the name, '{name}'", nameof(name));
        }
    }

    public IReadOnlyCollection<IOpaModule> Modules
        => modules.Values.ToList();
}
