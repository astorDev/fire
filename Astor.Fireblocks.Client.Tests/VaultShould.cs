using Shouldly;

namespace Astor.Fireblocks.Client.Tests;

[TestClass]
public class VaultShould : Test 
{
    [TestMethod]
    public async Task ReturnVaultAccountAsset()
    {
        var accountAsset = await Client.GetAccountAsset("61", "ETH_TEST5");
        accountAsset.Available.ShouldNotBe(0);
        accountAsset.ShouldNotBeNull();
    }
}