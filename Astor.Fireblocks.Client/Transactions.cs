using Nist.Queries;

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
    
    public async Task<TransactionDetails[]> GetTransactions(TransactionsQuery query) => 
        await this.GetAsync<TransactionDetails[]>(FireblocksUris.TransactionsV1, query);
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

public record TransactionsQuery(
    long? Before = null,
    long? After = null,
    int? Limit = null,
    string? Status = null,
    string? OrderBy = null,
    string? Sort = null,
    string? SourceType = null,
    string? SourceId = null,
    string? DestType = null,
    string? DestId = null,
    string? Assets = null,
    string? TxHash = null,
    string? SourceWalletId = null,
    string? DestWalletId = null
);

public class FireblocksTime
{
    public static long NowPlus(TimeSpan offset) => DateTimeOffset.UtcNow.Add(offset).ToUnixTimeMilliseconds();
}

public static class PeerTypes
{
    public const string VaultAccount = "VAULT_ACCOUNT";
    public const string OneTimeAddress = "ONE_TIME_ADDRESS";
}