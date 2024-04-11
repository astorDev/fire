using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Astor.Fireblocks.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFireblocks(this IServiceCollection services, IConfiguration namedConfigurationSection)
    {
        services.AddOptions<FireblocksClientOptions>()
            .Bind(namedConfigurationSection)
            .ValidateDataAnnotations();

        services.AddHttpClient<Sender>((sp, cl) =>
        {
            var options = sp.GetRequiredService<IOptions<FireblocksClientOptions>>();
            cl.BaseAddress = new(options.Value.Url);
        });
        services.AddScoped<FireblocksClient>();

        services.AddScoped<FireblocksAuthenticator>();

        return services;
    }
} 