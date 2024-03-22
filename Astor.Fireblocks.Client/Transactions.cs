namespace Astor.Fireblocks.Client;

public partial class FireblocksUris
{
    public const string Transactions = "transactions";
    public static readonly string TransactionsV1 = $"{V1}/{Transactions}";
}

public partial class FireblocksClient
{
    public async Task<object> PostTransaction(TransactionCandidate candidate) => 
        await PostAsync<object>(FireblocksUris.TransactionsV1, candidate);
}

public record TransactionCandidate(
    string AssetId,
    decimal Amount,
    TransactionSource Source,
    TransactionDestination Destination,
    string Note
);

public record TransactionSource(
    string Type,
    string Id
);

public record TransactionDestination(
    string Type,
    string Id
);

public static class PeerTypes
{
    public const string VaultAccount = "VAULT_ACCOUNT";
}