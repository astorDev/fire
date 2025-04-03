namespace Fire.Blocks;

public record TransactionWebhook(
    string Type,
    TransactionDetails Data
);

public class WebhookEventTypes {
    public const string TransactionStatusUpdated = "TRANSACTION_STATUS_UPDATED";
    public const string TransactionCreated = "TRANSACTION_CREATED";
}

