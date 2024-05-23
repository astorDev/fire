using FluentAssertions;

namespace Astor.Fireblocks.Client.Tests;

[TestClass]
public class VaultShould : Test 
{
    [TestMethod]
    public async Task ReturnVaultAccountAsset()
    {
        var accountAsset = await Client.GetAccountAsset("61", "ETH_TEST5");
        accountAsset.Available.Should().NotBe(0);
        accountAsset.Should().NotBeNull();
    }
}