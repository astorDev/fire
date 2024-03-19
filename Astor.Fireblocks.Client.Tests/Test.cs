using dotenv.net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astor.Fireblocks.Client.Tests;

public class Test 
{
    public IServiceProvider Services { get; }

    public Test()
    {
        DotEnv.Load();
        var configuration = new ConfigurationManager();
        configuration.AddEnvironmentVariables();
        var services = new ServiceCollection();
        services.AddFireblocks(configuration.GetSection("Fireblocks"));
        Services = services.BuildServiceProvider();
    }
}