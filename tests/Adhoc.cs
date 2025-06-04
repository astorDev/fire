using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Fire;

namespace Fire.Blocks.Tests;

[TestClass]
public class Adhoc : Test
{
    private readonly Dotenvs envs = Dotenvs.FromFiles("adhoc.env");

    [TestMethod]
    public async Task GetTransaction()
    {
        var transactions = await Client.GetTransaction(envs["TRANSACTION_ID"]);
        Console.WriteLine(JsonSerializer.Serialize(transactions));
    }
}

public record Dotenvs(IDictionary<string, string> Values)
{
    public static Dotenvs FromFiles(params string[] paths)
    {
        var values = dotenv.net.DotEnv.Read(new dotenv.net.DotEnvOptions(envFilePaths: paths, ignoreExceptions: false));
        return new Dotenvs(values);
    }

    public string this[string key]
    {
        get
        {
            if (Values.TryGetValue(key, out var value))
                return value;

            throw new KeyNotFoundException($"Key '{key}' not found in the environment variables.");

        }
    }
}