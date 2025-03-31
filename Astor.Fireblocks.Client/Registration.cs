using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Astor.Fireblocks.Client;

public record FireblocksClientOptions
{
    public string? Url { get; set; }

    [Required]
    public required string ApiKey { get; set; }

    [Required]
    public required string ApiSecret { get; set; }
}

public static class Registration
{
    public static IServiceCollection AddFireblocks(
        this IServiceCollection services, 
        IConfiguration namedConfigurationSection,
        Func<string, RsaSecurityKey>? rsaKeyResolution = null)
    {
        return services.AddFireblocks(o => o.Bind(namedConfigurationSection), rsaKeyResolution);
    }

    public static IServiceCollection AddFireblocks(this IServiceCollection services, 
        Action<OptionsBuilder<FireblocksClientOptions>> optionsConfiguration,
        Func<string, RsaSecurityKey>? rsaKeyResolution = null)
    {
        rsaKeyResolution ??= DefaultSecurityKeyResolution;

        return services.AddFireblocks(
            optionsConfiguration,
            o => o.ApiKey,
            o => rsaKeyResolution(o.ApiSecret),
            o => o.Url == null ? new Uri("https://api.fireblocks.io") : new Uri(o.Url)
        );
    }

    public static IServiceCollection AddFireblocks(
        this IServiceCollection services,
        Action<OptionsBuilder<FireblocksClientOptions>> optionsConfiguration,
        Func<FireblocksClientOptions, string> apiKeyResolution,
        Func<FireblocksClientOptions, RsaSecurityKey> privateKeyResolution,
        Func<FireblocksClientOptions, Uri> baseUriResolution
        )
    {
        var optionsBuilder = services.AddOptions<FireblocksClientOptions>();
        optionsConfiguration(optionsBuilder);
        optionsBuilder.ValidateDataAnnotations();

        return services.AddFireblocks(
            sp =>
            {
                var options = sp.GetRequiredService<IOptionsSnapshot<FireblocksClientOptions>>().Value;
                return apiKeyResolution(options);
            },
            sp =>
            {
                var options = sp.GetRequiredService<IOptionsSnapshot<FireblocksClientOptions>>().Value;
                return privateKeyResolution(options);
            },
            sp =>
            {
                var options = sp.GetRequiredService<IOptionsSnapshot<FireblocksClientOptions>>().Value;
                return baseUriResolution(options);
            }
        );
    }
    
    public static IServiceCollection AddFireblocks(
        this IServiceCollection services,
        Func<IServiceProvider, string> apiKeyResolution,
        Func<IServiceProvider, RsaSecurityKey> privateKeyResolution,
        Func<IServiceProvider, Uri> baseUriResolution)
    {
        services.AddHttpClient<Sender>((sp, cl) =>
        {
            var uri = baseUriResolution(sp);
            cl.BaseAddress = uri;
        });
        services.AddScoped<FireblocksClient>();
        services.AddScoped<FireblocksAuthenticator>((sp) =>
        {
            var apiKey = apiKeyResolution(sp);
            var privateKey = privateKeyResolution(sp);
            return new(apiKey, privateKey);
        });

        return services;
    }

    public static RsaSecurityKey DefaultSecurityKeyResolution(string secretFromConfiguration)
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(secretFromConfiguration);
        return new(rsa);
    }
} 