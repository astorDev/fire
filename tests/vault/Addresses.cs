namespace Fire.Blocks.Tests;

[TestClass]
public class Addresses : Test
{
    [TestMethod]
    public async Task GetAddresses()
    {
        const string assetId = "USDC_BASECHAIN_ETH_TEST5_8SH8";

        var accountName = Guid.NewGuid().ToString();
        var account = await Client.PostAccount(new(Name: accountName));
        Console.WriteLine($"Created account: {account.Id} - {account.Name}");

        var addresses = await Client.GetAccountAssetAddresses(account.Id, assetId);
        Console.WriteLine($"Found {addresses.Length} for account {account.Id} ({accountName})");

        _ = await Client.PostAccountAsset(account.Id, assetId, new());

        addresses = await Client.GetAccountAssetAddresses(account.Id, assetId);
        Console.WriteLine($"Found {addresses.Length} for account {account.Id} ({accountName})");
        
        foreach (var address in addresses)
        {
            Console.WriteLine($"Address: {address.Address}, Tag: {address.Tag}, Description: {address.Description}");
        }
    }
}