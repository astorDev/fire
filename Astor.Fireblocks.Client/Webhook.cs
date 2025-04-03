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
    public const string Rejected = "REJECTED";
}

public record TransactionDetails(
    string Id,
    string AssetId,
    PeerDetails Source,
    PeerDetails Destination,
    decimal RequestedAmount,
    decimal Amount,
    decimal NetAmount,
    decimal AmountUSD,
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
    string AssetType
);

public record AmountInfo(
    decimal Amount,
    decimal RequestedAmount,
    decimal NetAmount,
    decimal AmountUsd
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