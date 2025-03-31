using System.Globalization;
using dotenv.net;
using Fluenv;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Astor.Fireblocks.Client.Tests;

public class Test 
{
    public IServiceProvider Services { get; }
    public FireblocksClient Client { get; }

    private readonly string? _asset;
    public string Asset => _asset ?? throw new InvalidOperationException("Asset not set");

    private readonly string? _amount;
    public decimal Amount => _amount == null ? throw new InvalidOperationException("Amount not set") : Decimal.Parse(_amount, CultureInfo.InvariantCulture);

    private readonly string? _threshold;
    public decimal Threshold => _threshold == null ? throw new InvalidOperationException("Threshold not set") : Decimal.Parse(_threshold, CultureInfo.InvariantCulture);

    private readonly string? _externalReceiver;
    public string ExternalReceiver => _externalReceiver ?? throw new InvalidOperationException("ExternalReceiver not set");

    public Test()
    {
        DotEnv.Load();
        var configuration = new ConfigurationManager();
        configuration.AddFluentEnvironmentVariables();
        var services = new ServiceCollection();
        services.AddLogging(l =>
        {
            l.AddSimpleConsole(c => c.SingleLine = true);
            l.SetMinimumLevel(LogLevel.Debug);
        });

        services.AddFireblocks(configuration.GetSection("Fireblocks"));
            
        Services = services.BuildServiceProvider();
        Client = Services.GetRequiredService<FireblocksClient>();
        
        _asset = configuration["Asset"];
        _amount = configuration["Amount"];
        _threshold = configuration["Threshold"];
        _externalReceiver = configuration["ExternalReceiver"];
    }
}