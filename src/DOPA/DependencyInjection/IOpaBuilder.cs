using System.Text.Json;

namespace DOPA.DependencyInjection;

public interface IOpaBuilder
{
    IOpaBuilder UseSerializerOptions(JsonSerializerOptions options);

    IOpaBuilder LazyLoadModule();
}
