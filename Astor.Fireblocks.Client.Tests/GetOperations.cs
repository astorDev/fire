using System.Text.Json;
using Astor.Fireblocks.Client.Tests;
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
        await Client.GetAccountsPaged(new (
            NamePrefix: "hot-wallet"
        ));
    }
}