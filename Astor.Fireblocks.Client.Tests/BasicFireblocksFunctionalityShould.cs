using FluentAssertions;

namespace Astor.Fireblocks.Client.Tests;

[TestClass]
public class BasicFireblocksFunctionalityShould : Test
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

    [TestMethod]
    public async Task PerformTransfer()
    {
        var page = await Client.GetAccountsPaged(new (MinAmountThreshold: 10));
        var fullAssetsOrdered = page.Accounts
            .SelectMany(account => account.Assets.Select(asset => new { account, asset }))
            .OrderByDescending(fullAsset => fullAsset.asset.Balance)
            .ToArray();

        var richestAsset = fullAssetsOrdered.First();
        var poorestAsset = fullAssetsOrdered.Last();

        var transaction = new TransactionCandidate(
            AssetId: richestAsset.asset.Id,
            Amount: 1,
            Source: new(
                PeerTypes.VaultAccount,
                richestAsset.account.Id
            ),
            Destination: new(
                PeerTypes.VaultAccount,
                poorestAsset.account.Id
            ),
            Note: "test transfer of 1 from richest to poorest"
        );

        await Client.PostTransaction(transaction);
    }
}