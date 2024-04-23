namespace Astor.Fireblocks.Client;

public partial class FireblocksUris
{
    public const string Transactions = "transactions";
    public static readonly string TransactionsV1 = $"{V1}/{Transactions}";
}

public partial class FireblocksClient
{
    public async Task<CreatedTransaction> PostTransaction(TransactionCandidate candidate) => 
        await PostAsync<CreatedTransaction>(FireblocksUris.TransactionsV1, candidate);

    public async Task<TransactionDetails> GetTransaction(string transactionId) =>
        await GetAsync<TransactionDetails>($"{FireblocksUris.TransactionsV1}/{transactionId}");
}

public record TransactionCandidate(
    string AssetId,
    decimal Amount,
    TransactionSource Source,
    TransactionDestination Destination,
    string Note
);

public record CreatedTransaction(
    string Id,
    string Status
);

public record TransactionSource(
    string Type,
    string Id
);

public record TransactionDestination(
    string Type,
    string? Id = null,
    OneTimeAddress? OneTimeAddress = null
);

public record OneTimeAddress(
    string Address
);

public static class PeerTypes
{
    public const string VaultAccount = "VAULT_ACCOUNT";
    public const string OneTimeAddress = "ONE_TIME_ADDRESS";
}