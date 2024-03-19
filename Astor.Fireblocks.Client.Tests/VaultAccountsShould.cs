using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Astor.Fireblocks.Client.Tests;

[TestClass]
public class VaultAccountsShould : Test
{
    [TestMethod]
    public void ReturnAccounts()
    {
        var settings = Services.GetRequiredService<IOptions<FireblocksClientOptions>>().Value;
        Console.WriteLine(settings);
    }
}