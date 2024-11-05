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
    public const string Submitted = "SUBMITTED";
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
    BlockInfo BlockInfo,
    int NumOfConfirmations,
    string DestinationTag,
    DestinationDetails[] Destinations
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