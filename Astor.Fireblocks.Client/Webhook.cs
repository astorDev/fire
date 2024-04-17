namespace Astor.Fireblocks;

public record TransactionWebhook(
    string Type,
    TransactionDetails Data
);

public class WebhookEventTypes {
    public const string TransactionStatusUpdated = "TRANSACTION_STATUS_UPDATED";
    public const string TransactionCreated = "TRANSACTION_CREATED";
}

public class TransactionStatus {
    public const string Confirming = "CONFIRMING";
    public const string Completed = "COMPLETED";
}

public record TransactionDetails(
    string Id,
    long CreatedAt,
    long LastUpdated,
    string AssetId,
    PeerDetails Source,
    PeerDetails Destination,
    decimal Amount,
    string SourceAddress,
    string DestinationAddress,
    string Status,
    string TxHash,
    string SubStatus,
    string FeeCurrency,
    BlockInfo BlockInfo
);

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