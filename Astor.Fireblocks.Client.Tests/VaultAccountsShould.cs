using FluentAssertions;

namespace Astor.Fireblocks.Client.Tests;

[TestClass]
public class VaultAccountsShould : Test
{
    [TestMethod]
    public async Task ReturnAccounts()
    {
        var page = await Client.GetAccountsPaged();
        page.Accounts.Length.Should().NotBe(0);
    }

    [TestMethod]
    public async Task AllowFilteringByMinAmountThreshold()
    {
        var page = await Client.GetAccountsPaged(new (MinAmountThreshold: 10));
        var onlyValidAssets = page.Accounts.All(a => a.Assets.All(a => a.Balance >= 10));
        onlyValidAssets.Should().BeTrue();
    }
}