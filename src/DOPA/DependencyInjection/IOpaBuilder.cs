using System.Text.Json;

namespace DOPA.DependencyInjection;

/// <summary>
/// A builder for putting together an <see cref="IOpaModule" />.
/// </summary>
public interface IOpaBuilder
{
    /// <summary>
    /// Use these <see cref="JsonSerializerOptions" />.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <returns>The builder.</returns>
    IOpaBuilder UseSerializerOptions(JsonSerializerOptions options);

    /// <summary>
    /// Lazy load the module instead of eager loading.
    /// </summary>
    /// <returns>The builder.</returns>
    IOpaBuilder LazyLoadModule();
}
