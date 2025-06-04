namespace Fire.Blocks;

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
    string Note,
    string? ExternalTxId = null
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

public class TransactionStatus {
    public const string Submitted = "SUBMITTED";
    public const string PendingAmlScreening = "PENDING_AML_SCREENING";
    public const string PendingEnrichment = "PENDING_ENRICHMENT";
    public const string PendingAuthorization = "PENDING_AUTHORIZATION";
    public const string Queued = "QUEUED";
    public const string PendingSignature = "PENDING_SIGNATURE";
    public const string Pending3rdPartyManualApproval = "PENDING_3RD_PARTY_MANUAL_APPROVAL";
    public const string Pending3rdParty = "PENDING_3RD_PARTY";
    public const string Broadcasting = "BROADCASTING";
    public const string Confirming = "CONFIRMING";
    public const string Completed = "COMPLETED";

    /// <summary>
    /// The Cancelling status indicates a transaction was canceled or rejected by a Fireblocks user, 
    // such as the transaction’s initiator, approver, or designated signer.
    //
    // Typically, a transaction should remain in this status for no longer than 30 seconds, 
    // but it depends on the current load on the Fireblocks system.
    /// </summary>
    public const string Cancelling = "CANCELLING";

    /// <summary>
    /// The Cancelled status indicates a transaction was canceled or rejected by a Fireblocks user, 
    // such as the transaction’s initiator, approver, or designated signer, 
    // or by the third-party service that was the source of the transaction.
    //
    // Cancelled is a final transaction status and marks the transaction as unsuccessful. 
    // After a transaction is marked as canceled, 
    // all associated assets are available for new transactions.
    /// </summary>
    public const string Cancelled = "CANCELLED";

    /// <summary>
    /// The Blocked by policy status indicates an outgoing transaction was blocked 
    // from being completed due to a TAP rule.
    /// 
    // Appears in the Fireblocks Console as: Blocked by policy
    // Blocked by policy is a final transaction status and marks the transaction as unsuccessful. 
    // After a transaction is marked as blocked, 
    // all associated assets are available for new transactions.
    /// </summary>
    public const string BlockedByPolicy = "BLOCKED_BY_POLICY";

    /// <summary>
    /// Appears in the Fireblocks Console as: Rejected by AML, Manually frozen
    /// </summary>
    public const string Rejected = "REJECTED";
    public const string Failed = "FAILED";

}

public record TransactionDetails(
    string Id,
    string AssetId,
    PeerDetails Source,
    PeerDetails Destination,
    decimal RequestedAmount,
    decimal Amount,
    decimal NetAmount,
    decimal? AmountUSD,
    decimal Fee,
    decimal NetworkFee,
    long CreatedAt,
    long LastUpdated,
    string Status,
    string TxHash,
    string SubStatus,
    string SourceAddress,
    string DestinationAddress,
    string DestinationAddressDescription,
    string DestinationTag,
    string[] SignedBy,
    string CreatedBy,
    string RejectedBy,
    string AddressType,
    string Note,
    string ExchangeTxId,
    string FeeCurrency,
    string Operation,
    int NumOfConfirmations,
    AmountInfo AmountInfo,
    FeeInfo FeeInfo,
    // signedMessages - array of unknown type
    DestinationDetails[] Destinations,
    BlockInfo BlockInfo,
    int Index,
    string AssetType,
    string? ExternalTxId
);

public record AmountInfo(
    decimal Amount,
    decimal RequestedAmount,
    decimal NetAmount,
    decimal? AmountUsd
);

public record FeeInfo(
    decimal NetworkFee,
    decimal GasPrice
);

public record DestinationDetails(
    decimal Amount,
    PeerDetails Destination,
    decimal AmountUsd,
    string DestinationAddress,
    string DestinationAddressDescription,
    AmlScreeningResult AmlScreeningResult,
    string customerRefId
);

public record AmlScreeningResult(
    string Provider,
    dynamic Payload,
    string ScreeningStatus,
    string Verdict
);

public class AmlScreeningVerdict
{
    public const string Accept = "ACCEPT";
    public const string Reject = "REJECT";
    public const string Alert = "ALERT";
}

public record PeerDetails(
    string Id,
    string Type,
    string Name,
    string SubType
);

public record BlockInfo(
    string BlockHeight,
    string BlockHash
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

public partial class ErrorCodes {
    public const int ExternalTxIdDuplicate = 1438;
}