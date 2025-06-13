namespace Fire.Blocks;

public record TransactionWebhook(
    string Type,
    TransactionDetails Data
);

public class WebhookEventTypes {
    public const string TransactionStatusUpdated = "TRANSACTION_STATUS_UPDATED";
    public const string TransactionCreated = "TRANSACTION_CREATED";
}

public partial class FireblocksUris
{
    public const string Webhooks = "webhooks";
    public const string Resend = "resend";
    public static string V1Webhooks => $"{V1}/{Webhooks}";
    public static string V1WebhooksResend => $"{V1Webhooks}/{Resend}";
}

public record WebhooksResent(
    int MessageCount
);

public partial class FireblocksClient {
    public async Task<WebhooksResent> PostWebhooksResend() =>
        await PostAsync<WebhooksResent>(FireblocksUris.V1WebhooksResend, new { });
}