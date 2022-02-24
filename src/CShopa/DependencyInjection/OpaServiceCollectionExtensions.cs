using CShopa.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace CShopa.DependencyInjection;

public static class OpaServiceCollectionExtensions
{
    public static IServiceCollection AddOpaPolicy(this IServiceCollection services, string wasmFilePath)
    {
        var module = WasmModule.FromFile(wasmFilePath);
        services.AddTransient<IOpaPolicy>(_ => module.CreatePolicy());
        return services;
    }

    public static IServiceCollection AddOpaPolicy(this IServiceCollection services, byte[] wasmContent)
    {
        var module = WasmModule.FromBytes("WasmModule", wasmContent);
        services.AddTransient<IOpaPolicy>(_ => module.CreatePolicy());
        return services;
    }

    public static IServiceCollection AddOpaPolicy(this IServiceCollection services, Stream stream)
    {
        var module = WasmModule.FromStream("WasmModule", stream);
        services.AddTransient<IOpaPolicy>(_ => module.CreatePolicy());
        return services;
    }

    public static IServiceCollection AddOpaPolicy<TConsumer>(this IServiceCollection services, string wasmFilePath)
    {
        var module = WasmModule.FromFile(wasmFilePath);
        services.AddTransient<IOpaPolicy<TConsumer>>(_ => new OpaPolicy<TConsumer>(module.CreatePolicy()));
        return services;
    }

    public static IServiceCollection AddOpaPolicy<TConsumer>(this IServiceCollection services, byte[] wasmContent)
    {
        var module = WasmModule.FromBytes(nameof(TConsumer), wasmContent);
        services.AddTransient<IOpaPolicy<TConsumer>>(_ => new OpaPolicy<TConsumer>(module.CreatePolicy()));
        return services;
    }

    public static IServiceCollection AddOpaPolicy<TConsumer>(this IServiceCollection services, Stream stream)
    {
        var module = WasmModule.FromStream(nameof(TConsumer), stream);
        services.AddTransient<IOpaPolicy<TConsumer>>(_ => new OpaPolicy<TConsumer>(module.CreatePolicy()));
        return services;
    }
}
