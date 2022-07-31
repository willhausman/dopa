namespace DOPA;

/// <inheritdoc />
public class OpaModuleCollection : IOpaModuleCollection
{
    private readonly IDictionary<string, IOpaModule> modules;

    /// <summary>
    /// Initializes the class.
    /// </summary>
    /// <param name="modules">An enumerable of loaded modules.</param>
    public OpaModuleCollection(IEnumerable<IOpaModule> modules)
    {
        this.modules = modules.ToDictionary(m => m.Name, StringComparer.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public IReadOnlyCollection<IOpaModule> Modules
        => modules.Values.ToList();
}
