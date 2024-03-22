namespace Astor.Fireblocks.Client;

public partial class FireblocksUris
{
    public const string Vault = "vault";
    public const string AccountsPaged = "accounts_paged";
    public static readonly string VaultAccountsPaged = $"{Vault}/{AccountsPaged}";
    public static readonly string VaultAccountsPagedV1 = $"{V1}/{VaultAccountsPaged}";
}

public partial class FireblocksClient
{
    public async Task<VaultAccountsPaginated> GetAccountsPaged(PaginatedVaultAccountsQuery? query = null) => 
        await GetAsync<VaultAccountsPaginated>(FireblocksUris.VaultAccountsPagedV1, query);
}

public record VaultAccountsPaginated(VaultAccount[] Accounts);

public record VaultAccount(
    string Id,
    string Name,
    VaultAccountAsset[] Assets
);

public record PaginatedVaultAccountsQuery(
    decimal? MinAmountThreshold = null
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