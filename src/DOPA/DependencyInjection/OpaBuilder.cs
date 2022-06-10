using System.Text.Json;
using DOPA.Serialization;

namespace DOPA.DependencyInjection;

internal class OpaBuilder : IOpaBuilder
{
    private IOpaSerializer? serializer;
    private readonly Func<IOpaModule> factory;

    public OpaBuilder(Func<IOpaModule> factory)
    {
        this.factory = factory;
        Module = new Lazy<IOpaModule>(Build);
    }

    public bool EagerLoad { get; private set; } = true;

    public Lazy<IOpaModule> Module { get; }

    public IOpaBuilder LazyLoadModule()
    {
        EagerLoad = false;
        return this;
    }

    public IOpaBuilder UseSerializerOptions(JsonSerializerOptions options)
    {
        serializer = new OpaSerializer(options);
        return this;
    }

    public IOpaModule Build()
    {
        var module = factory.Invoke();

        if (serializer is not null)
        {
            module.Serializer = serializer;
        }

        return module;
    }
}
