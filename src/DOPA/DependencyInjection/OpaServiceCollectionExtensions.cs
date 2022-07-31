using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DOPA.DependencyInjection;

/// <summary>
/// Convenience extension methods for using DOPA with Microsoft Dependency Injection.
/// </summary>
public static class OpaServiceCollectionExtensions
{
    /// <summary>
    /// Register <see cref="IOpaPolicy" /> and configure it.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="wasmFilePath">File path to the compiled .wasm file.</param>
    /// <param name="options">Optional action to configure policies.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddOpaPolicy(this IServiceCollection services, string wasmFilePath, Action<IOpaBuilder>? options = null)
    {
        services
            .ConfigureModule(wasmFilePath, () => WasmModule.FromFile(wasmFilePath), options)
            .AddTransientOpaPolicy(wasmFilePath);
        return services;
    }

    /// <summary>
    /// Register <see cref="IOpaPolicy" /> and configure it.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="name">A name for the policy.</param>
    /// <param name="wasmContent">Contents of the compiled .wasm file.</param>
    /// <param name="options">Optional action to configure policies.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddOpaPolicy(this IServiceCollection services, string name, byte[] wasmContent, Action<IOpaBuilder>? options = null)
    {
        services
            .ConfigureModule(name, () => WasmModule.FromBytes(name, wasmContent), options)
            .AddTransientOpaPolicy(name);
        return services;
    }

    /// <summary>
    /// Register <see cref="IOpaPolicy" /> and configure it.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="name">A name for the policy.</param>
    /// <param name="stream">A <see cref="Stream" /> with the .wasm contents.</param>
    /// <param name="options">Optional action to configure policies.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddOpaPolicy(this IServiceCollection services, string name, Stream stream, Action<IOpaBuilder>? options = null)
    {
        services
            .ConfigureModule(name, () => WasmModule.FromStream(name, stream), options)
            .AddTransientOpaPolicy(name);
        return services;
    }


    /// <summary>
    /// Register <see cref="IOpaPolicy" /> and configure it.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="wasmFilePath">File path to the compiled .wasm file.</param>
    /// <param name="options">Optional action to configure policies.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddOpaPolicy<TModuleName>(this IServiceCollection services, string wasmFilePath, Action<IOpaBuilder>? options = null)
    {
        var name = nameof(TModuleName);
        services
            .ConfigureModule(name, () => WasmModule.FromFile(name, wasmFilePath), options)
            .AddTransientOpaPolicy<TModuleName>(name);
        return services;
    }

    /// <summary>
    /// Register <see cref="IOpaPolicy" /> and configure it.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="wasmContent">Contents of the compiled .wasm file.</param>
    /// <param name="options">Optional action to configure policies.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddOpaPolicy<TModuleName>(this IServiceCollection services, byte[] wasmContent, Action<IOpaBuilder>? options = null)
    {
        var name = nameof(TModuleName);
        services
            .ConfigureModule(name, () => WasmModule.FromBytes(name, wasmContent), options)
            .AddTransientOpaPolicy<TModuleName>(name);
        return services;
    }

    /// <summary>
    /// Register <see cref="IOpaPolicy" /> and configure it.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="stream">A <see cref="Stream" /> with the .wasm contents.</param>
    /// <param name="options">Optional action to configure policies.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddOpaPolicy<TModuleName>(this IServiceCollection services, Stream stream, Action<IOpaBuilder>? options = null)
    {
        var name = nameof(TModuleName);
        services
            .ConfigureModule(name, () => WasmModule.FromStream(name, stream), options)
            .AddTransientOpaPolicy<TModuleName>(name);
        return services;
    }

    private static IServiceCollection ConfigureModule(this IServiceCollection services, string name, Func<IOpaModule> factory, Action<IOpaBuilder>? options)
    {
        var builder = new OpaBuilder(factory);

        options?.Invoke(builder);

        if (builder.EagerLoad)
        {
            var _ = builder.Module.Value;
        }

        services
            .AddSingleton(_ => builder.Module.Value)
            .TryAddTransient<IOpaModuleCollection, OpaModuleCollection>()
            ;

        return services;
    }

    private static IServiceCollection AddTransientOpaPolicy(this IServiceCollection services, string name)
    {
        return services.AddTransient(provider =>
            provider.GetRequiredService<IOpaModuleCollection>()[name].CreatePolicy());
    }

    private static IServiceCollection AddTransientOpaPolicy<TModuleName>(this IServiceCollection services, string name)
    {
        return services.AddTransient<IOpaPolicy<TModuleName>>(provider =>
            new OpaPolicy<TModuleName>(provider.GetRequiredService<IOpaModuleCollection>()[name].CreatePolicy()));
    }
}
