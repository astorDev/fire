using dotenv.net;
using Fluenv;
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
        configuration.AddFluentEnvironmentVariables();
        var services = new ServiceCollection();
        services.AddFireblocks(configuration.GetSection("Fireblocks"));
        Services = services.BuildServiceProvider();
    }
}