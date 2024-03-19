using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astor.Fireblocks.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFireblocks(this IServiceCollection services, IConfiguration namedConfigurationSection)
    {
        services.AddOptions<FireblocksClientOptions>()
            .Bind(namedConfigurationSection)
            .ValidateDataAnnotations();

        return services;
    }
} 