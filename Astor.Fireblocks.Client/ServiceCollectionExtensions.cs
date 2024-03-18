using Microsoft.Extensions.DependencyInjection;

namespace Astor.Fireblocks.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFireblocks(this IServiceCollection services)
    {
        return services;
    }
} 