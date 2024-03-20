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

    public Test()
    {
        DotEnv.Load();
        var configuration = new ConfigurationManager();
        configuration.AddFluentEnvironmentVariables();
        var services = new ServiceCollection();
        services.AddLogging(l => l.AddSimpleConsole(c => c.SingleLine = true));
        services.AddFireblocks(configuration.GetSection("Fireblocks"));
        Services = services.BuildServiceProvider();
        Client = Services.GetRequiredService<FireblocksClient>();
    }
}