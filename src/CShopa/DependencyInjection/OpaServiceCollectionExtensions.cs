using CShopa.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CShopa.DependencyInjection;

public static class OpaServiceCollectionExtensions
{
    public static IServiceCollection AddOpaPolicy(this IServiceCollection services, string wasmFilePath)
    {
        var module = WasmModule.FromFile(wasmFilePath);
        services.AddSingleton<IOpaModule>(_ => module);
        services.TryAddTransient<IOpaModuleCollection, OpaModuleCollection>();

        services.AddTransient<IOpaPolicy>(provider =>
            provider.GetRequiredService<IOpaModuleCollection>()[wasmFilePath].CreatePolicy());
        return services;
    }

    public static IServiceCollection AddOpaPolicy(this IServiceCollection services, string name, byte[] wasmContent)
    {
        var module = WasmModule.FromBytes(name, wasmContent);
        services.AddSingleton<IOpaModule>(_ => module);
        services.TryAddTransient<IOpaModuleCollection, OpaModuleCollection>();

        services.AddTransient<IOpaPolicy>(provider =>
            provider.GetRequiredService<IOpaModuleCollection>()[name].CreatePolicy());
        return services;
    }

    public static IServiceCollection AddOpaPolicy(this IServiceCollection services, string name, Stream stream)
    {
        var module = WasmModule.FromStream(name, stream);
        services.AddSingleton<IOpaModule>(_ => module);
        services.TryAddTransient<IOpaModuleCollection, OpaModuleCollection>();

        services.AddTransient<IOpaPolicy>(provider =>
            provider.GetRequiredService<IOpaModuleCollection>()[name].CreatePolicy());
        return services;
    }

    public static IServiceCollection AddOpaPolicy<TModuleName>(this IServiceCollection services, string wasmFilePath)
    {
        var name = nameof(TModuleName);
        var module = WasmModule.FromFile(name, wasmFilePath);
        services.AddSingleton<IOpaModule>(_ => module);
        services.TryAddTransient<IOpaModuleCollection, OpaModuleCollection>();

        services.AddTransient<IOpaPolicy<TModuleName>>(provider =>
            new OpaPolicy<TModuleName>(provider.GetRequiredService<IOpaModuleCollection>()[name].CreatePolicy()));
        return services;
    }

    public static IServiceCollection AddOpaPolicy<TModuleName>(this IServiceCollection services, byte[] wasmContent)
    {
        var name = nameof(TModuleName);
        var module = WasmModule.FromBytes(name, wasmContent);
        services.AddSingleton<IOpaModule>(_ => module);
        services.TryAddTransient<IOpaModuleCollection, OpaModuleCollection>();

        services.AddTransient<IOpaPolicy<TModuleName>>(provider =>
            new OpaPolicy<TModuleName>(provider.GetRequiredService<IOpaModuleCollection>()[name].CreatePolicy()));
        return services;
    }

    public static IServiceCollection AddOpaPolicy<TModuleName>(this IServiceCollection services, Stream stream)
    {
        var name = nameof(TModuleName);
        var module = WasmModule.FromStream(name, stream);
        services.AddSingleton<IOpaModule>(_ => module);
        services.TryAddTransient<IOpaModuleCollection, OpaModuleCollection>();

        services.AddTransient<IOpaPolicy<TModuleName>>(provider =>
            new OpaPolicy<TModuleName>(provider.GetRequiredService<IOpaModuleCollection>()[name].CreatePolicy()));
        return services;
    }
}
