using CShopa.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CShopa.DependencyInjection;

public static class OpaServiceCollectionExtensions
{
    public static IServiceCollection AddOpaPolicy(this IServiceCollection services, string wasmFilePath, Action<IOpaBuilder>? options = null)
    {
        services
            .ConfigureModule(wasmFilePath, () => WasmModule.FromFile(wasmFilePath), options)
            .AddTransientOpaPolicy(wasmFilePath);
        return services;
    }

    public static IServiceCollection AddOpaPolicy(this IServiceCollection services, string name, byte[] wasmContent, Action<IOpaBuilder>? options = null)
    {
        services
            .ConfigureModule(name, () => WasmModule.FromBytes(name, wasmContent), options)
            .AddTransientOpaPolicy(name);
        return services;
    }

    public static IServiceCollection AddOpaPolicy(this IServiceCollection services, string name, Stream stream, Action<IOpaBuilder>? options = null)
    {
        services
            .ConfigureModule(name, () => WasmModule.FromStream(name, stream), options)
            .AddTransientOpaPolicy(name);
        return services;
    }

    public static IServiceCollection AddOpaPolicy<TModuleName>(this IServiceCollection services, string wasmFilePath, Action<IOpaBuilder>? options = null)
    {
        var name = nameof(TModuleName);
        services
            .ConfigureModule(name, () => WasmModule.FromFile(name, wasmFilePath), options)
            .AddTransientOpaPolicy<TModuleName>(name);
        return services;
    }

    public static IServiceCollection AddOpaPolicy<TModuleName>(this IServiceCollection services, byte[] wasmContent, Action<IOpaBuilder>? options = null)
    {
        var name = nameof(TModuleName);
        services
            .ConfigureModule(name, () => WasmModule.FromBytes(name, wasmContent), options)
            .AddTransientOpaPolicy<TModuleName>(name);
        return services;
    }

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

        if (options is not null)
        {
            options(builder);
        }

        if (builder.EagerLoad)
        {
            var module = builder.Module.Value;
        }

        services
            .AddSingleton<IOpaModule>(_ => builder.Module.Value)
            .TryAddTransient<IOpaModuleCollection, OpaModuleCollection>()
            ;

        return services;
    }

    private static IServiceCollection AddTransientOpaPolicy(this IServiceCollection services, string name)
    {
        return services.AddTransient<IOpaPolicy>(provider =>
            provider.GetRequiredService<IOpaModuleCollection>()[name].CreatePolicy());
    }

    private static IServiceCollection AddTransientOpaPolicy<TModuleName>(this IServiceCollection services, string name)
    {
        return services.AddTransient<IOpaPolicy<TModuleName>>(provider =>
            new OpaPolicy<TModuleName>(provider.GetRequiredService<IOpaModuleCollection>()[name].CreatePolicy()));
    }
}
