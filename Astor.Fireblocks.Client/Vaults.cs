namespace Astor.Fireblocks.Client;

public partial class FireblocksUris
{
    public const string Vault = "vault";
    public const string AccountsPaged = "accounts_paged";
    public const string Accounts = "accounts";
    public static readonly string VaultAccountsPaged = $"{Vault}/{AccountsPaged}";
    public static readonly string VaultAccountsPagedV1 = $"{V1}/{VaultAccountsPaged}";
    public static readonly string VaultAccountsV1 = $"{V1}/{Vault}/{Accounts}";
}

public partial class FireblocksClient
{
    public async Task<VaultAccountsPaginated> GetAccountsPaged(PaginatedVaultAccountsQuery? query = null) => 
        await GetAsync<VaultAccountsPaginated>(FireblocksUris.VaultAccountsPagedV1, query);

    public async Task<VaultAccount> PostAccount(VaultAccountCandidate candidate) => 
        await PostAsync<VaultAccount>(FireblocksUris.VaultAccountsV1, candidate);
}

public record VaultAccountsPaginated(VaultAccount[] Accounts);

public record VaultAccount(
    string Id,
    string Name,
    bool HiddenOnUI,
    VaultAccountAsset[] Assets
);

public record VaultAccountCandidate(
    string Name,
    bool HiddenOnUI = false,
    string? CustomerRefIfSet = null,
    bool AutoFuel = false
);

public record PaginatedVaultAccountsQuery(
    string? MinAmountThreshold = null, // Workaround for problem with Nist.Queries work with decimal
    string? AssetId = null
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