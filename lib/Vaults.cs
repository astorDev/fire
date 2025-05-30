namespace Fire.Blocks;

public partial class FireblocksUris
{
    public const string Vault = "vault";
    public const string AccountsPaged = "accounts_paged";
    public const string Accounts = "accounts";
    public const string Addresses = "addresses";
    public static readonly string VaultAccountsPaged = $"{Vault}/{AccountsPaged}";
    public static readonly string VaultAccountsPagedV1 = $"{V1}/{VaultAccountsPaged}";
    public static readonly string VaultAccountsV1 = $"{V1}/{Vault}/{Accounts}";

    public static string VaultAccountV1(string accountId) => $"{VaultAccountsV1}/{accountId}";
    public static string VaultAccountAssetV1(string accountId, string assetId) => $"{VaultAccountsV1}/{accountId}/{assetId}";
    public static string VaultAccountAssetAddressesV1(string accountId, string assetId) => $"{VaultAccountAssetV1(accountId, assetId)}/{Addresses}";
}

public partial class FireblocksClient
{
    public async Task<VaultAccountsPaginated> GetAccountsPaged(PaginatedVaultAccountsQuery? query = null) =>
        await GetAsync<VaultAccountsPaginated>(FireblocksUris.VaultAccountsPagedV1, query);

    public async Task<VaultAccount> PostAccount(VaultAccountCandidate candidate) =>
        await PostAsync<VaultAccount>(FireblocksUris.VaultAccountsV1, candidate);

    public async Task<CreatedVaultAccountAsset> PostAccountAsset(string accountId, string assetId, VaultAccountAssetCandidate candidate) =>
        await PostAsync<CreatedVaultAccountAsset>(FireblocksUris.VaultAccountAssetV1(accountId, assetId), candidate);

    public async Task<VaultAccountAsset> GetAccountAsset(string accountId, string assetId) =>
        await GetAsync<VaultAccountAsset>(FireblocksUris.VaultAccountAssetV1(accountId, assetId));

    public async Task<VaultAccount> GetAccount(string accountId) =>
        await GetAsync<VaultAccount>(FireblocksUris.VaultAccountV1(accountId));

    public async Task<VaultAccountAddress[]> GetAccountAssetAddresses(string accountId, string assetId) =>
        await GetAsync<VaultAccountAddress[]>(FireblocksUris.VaultAccountAssetAddressesV1(accountId, assetId));
}

public record VaultAccountsPaginated(
    VaultAccount[] Accounts,
    Paging Paging,
    string PreviousUrl,
    string NextUrl);

public record Paging(string Before, string After);

public record VaultAccount(
    string Id,
    string Name,
    VaultAccountAsset[] Assets,
    bool HiddenOnUI,
    string CustomerRefId,
    bool AutoFuel
);

public record VaultAccountAddress(
    string AssetId,
    string Address,
    string Tag,
    string Description,
    string Type,
    string LegacyAddress,
    string EnterpriseAddress,
    int Bip44AddressIndex,
    bool UserDefined
);

public record VaultAccountCandidate(
    string Name,
    bool HiddenOnUI = false,
    string? CustomerRefIfSet = null,
    bool AutoFuel = false
);

public record PaginatedVaultAccountsQuery(
    string? MinAmountThreshold = null, // Workaround for problem with Nist.Queries work with decimal
    string? AssetId = null,
    string? After = null,
    int? Limit = null,
    string? Before = null,
    string? OrderBy = null,
    string? NameSuffix = null,
    string? NamePrefix = null
);

public record VaultAccountAssetCandidate(
    string? EosAccountName = null
);

public record CreatedVaultAccountAsset(
    string Id,
    string Address,
    string? LegacyAddress,
    string? Tag,
    string? EosAccountName
);

public record VaultAccountAsset(
    string Id,
    decimal Total,
    decimal Balance,
    decimal LockedAmount,
    decimal Available,
    decimal Pending,
    decimal Frozen,
    decimal Staked,
    long BlockHeight
);