using System.Text.Json;
using Astor.Fireblocks.Client.Tests;
using Microsoft.IdentityModel.Tokens;
using Shouldly;

namespace Fire;

[TestClass]
public class GetOperations : Test
{
    [TestMethod]
    public async Task GetSupportedAssets()
    {
        await Client.GetSupportedAssets();
    }

    [TestMethod]
    public async Task GasStation()
    {
        var gasStation = await Client.GetGasStation();
        Console.WriteLine(JsonSerializer.Serialize(gasStation));
        gasStation.ShouldNotBeNull();
    }

    [TestMethod]
    public async Task GetHotWallet()
    {
        var accounts = await Client.GetAccountsPaged(new (
            NamePrefix: "hot-wallet"
        ));
        
        Console.WriteLine(JsonSerializer.Serialize(accounts));
    }

    [TestMethod]
    public async Task GetAccount()
    {
        var accounts = await Client.GetAccountsPaged(new (
            NamePrefix: "hot-wallet"
        ));

        var account = await Client.GetAccount(accounts.Accounts.First().Id);
        Console.WriteLine(JsonSerializer.Serialize(account));
    }
}