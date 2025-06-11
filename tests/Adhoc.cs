using System.Text.Json;

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

    [TestMethod]
    public async Task PostTransaction()
    {
        var candidate = Json.DeserializeFile<TransactionCandidate>("adhoc.transaction.json");
        var transactions = await Client.PostTransaction(candidate);
        Console.WriteLine(JsonSerializer.Serialize(transactions));
    }

    [TestMethod] public void PostTransactionSync() => PostTransaction().GetAwaiter().GetResult();
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