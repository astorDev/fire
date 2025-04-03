using System.Globalization;
using System.RandomExtension;
using Shouldly;

namespace Fire.Blocks;

[TestClass]
public class BasicFireblocksFunctionalityShould : Test
{
    public PaginatedVaultAccountsQuery AssetAccountAfterThresholdQuery => new(
        MinAmountThreshold: Threshold.ToString(CultureInfo.InvariantCulture),
        AssetId: Asset
    );

    [TestMethod]
    public async Task CreateAccountAndAsset()
    {
        var account = await Client.PostAccount(new($"test {Guid.NewGuid()}"));
        var asset = await Client.PostAccountAsset(account.Id, Asset, new());

        account.Name.ShouldStartWith("test ");
        asset.EosAccountName.ShouldBeNull();
    }

    [TestMethod]
    public async Task ReturnAllAccounts()
    {
        var page = await Client.GetAccountsPaged();
        page.Accounts.Length.ShouldNotBe(0);
    }

    [TestMethod]
    public async Task AllowAccountsFiltering()
    {
        var page = await Client.GetAccountsPaged(AssetAccountAfterThresholdQuery);
        var onlyValidAssets = page.Accounts.All(a => a.Assets.All(a => a.Balance >= Threshold && a.Id == Asset));
        onlyValidAssets.ShouldBeTrue();
    }

    [TestMethod]
    public async Task PerformInternalTransfer()
    {
        var page = await Client.GetAccountsPaged(AssetAccountAfterThresholdQuery);
        var ordered = OrderedByBalanceDescending(page);

        var richestAsset = ordered.First();
        var poorestAsset = ordered.Last();

        var transaction = new TransactionCandidate(
            AssetId: richestAsset.Asset.Id,
            Amount: Amount,
            Source: new(
                PeerTypes.VaultAccount,
                richestAsset.Account.Id
            ),
            Destination: new(
                PeerTypes.VaultAccount,
                poorestAsset.Account.Id
            ),
            Note: $"test transfer of {Amount} from richest to poorest"
        );

        await Client.PostTransaction(transaction);
    }

    [TestMethod]
    public async Task PerformAssetTransferToOneTimeAddress()
    {
        await PerformAssetTransferToOneTimeAddressInternal();
    }

    [TestMethod]
    public async Task GetPerformedAssetTransferInfoUpToHash()
    {
        var createdTransaction = await PerformAssetTransferToOneTimeAddressInternal();
        var txHash = "";
        var timeoutToken = new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token;

        do
        {
            await Task.Delay(100);
            var transaction = await Client.GetTransaction(createdTransaction.Id);
            txHash = transaction.TxHash;
        } while (txHash == "" && !timeoutToken.IsCancellationRequested);

        txHash.ShouldNotBeEmpty();
    }

    private async Task<CreatedTransaction> PerformAssetTransferToOneTimeAddressInternal()
    {
        var page = await Client.GetAccountsPaged(AssetAccountAfterThresholdQuery);
        var richest = OrderedByBalanceDescending(page).First();

        var coefficient = new Random().NextDouble(0.5, 1);
        var actualAmount = Amount * Convert.ToDecimal(coefficient);
        var transaction = new TransactionCandidate(
            AssetId: richest.Asset.Id,
            Amount: actualAmount,
            Source: new(
                PeerTypes.VaultAccount,
                richest.Account.Id
            ),
            Destination: new(
                PeerTypes.OneTimeAddress,
                OneTimeAddress: new(
                    Address: ExternalReceiver
                )
            ),
            Note: $"test transfer of {actualAmount} from richest to external"
        );

        return await Client.PostTransaction(transaction);
    }

    public static (VaultAccount Account, VaultAccountAsset Asset)[] OrderedByBalanceDescending(VaultAccountsPaginated page)
    {
        return page.Accounts
            .SelectMany(account => account.Assets.Select(asset => (account, asset)))
            .OrderByDescending(fullAsset => fullAsset.asset.Balance)
            .ToArray();
    }
}