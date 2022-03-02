using System.Text.Json;

namespace CShopa.DependencyInjection;

public interface IOpaBuilder
{
    IOpaBuilder UseSerializerOptions(JsonSerializerOptions options);

    IOpaBuilder LazyLoadModule();
}
