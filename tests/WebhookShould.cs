using System.Text.Json;

namespace Fire.Blocks;

[TestClass]
public class WebhookShould
{
    [TestMethod]
    public void Deserialize()
    {
        var example = """
{
    "type": "TRANSACTION_STATUS_UPDATED",
    "tenantId": "0939319a-b205-4e2e-9575-67ba3be5f38b",
    "timestamp": 1743674067989,
    "data": {
        "id": "5ba67e22-067e-4ce8-85dc-0b901af835ac",
        "createdAt": 1743674057408,
        "lastUpdated": 1743674057556,
        "assetId": "ETH_TEST5",
        "source": {
            "id": "",
            "type": "UNKNOWN",
            "name": "External",
            "subType": ""
        },
        "destination": {
            "id": "1232",
            "type": "VAULT_ACCOUNT",
            "name": "payment-wallet-0195fb11-c5b2-7efd-954c-1ea8446d44ae",
            "subType": ""
        },
        "amount": 0.01,
        "networkFee": 0.000232752042894,
        "netAmount": 0.01,
        "sourceAddress": "0x6a39933f2968490686Dfa8f70a2398F620df054D",
        "destinationAddress": "0xB2C56EeD59Be097A14Fa354EF79D424A7A475230",
        "destinationAddressDescription": "",
        "destinationTag": "",
        "status": "COMPLETED",
        "txHash": "0x885c10a341fbbd65c8c6128dd28c185ba66e6cc55a997ec7c84dc450bb6991eb",
        "subStatus": "CONFIRMED",
        "signedBy": [],
        "createdBy": "",
        "rejectedBy": "",
        "amountUSD": 18.17031972778025,
        "addressType": "",
        "note": "",
        "exchangeTxId": "",
        "requestedAmount": 0.01,
        "feeCurrency": "ETH_TEST5",
        "operation": "TRANSFER",
        "customerRefId": null,
        "numOfConfirmations": 1,
        "amountInfo": {
            "amount": "0.01",
            "requestedAmount": "0.01",
            "netAmount": "0.01",
            "amountUSD": "18.17031972778025"
        },
        "feeInfo": {
            "networkFee": "0.000232752042894",
            "gasPrice": "11.083430614"
        },
        "destinations": [],
        "externalTxId": null,
        "blockInfo": {
            "blockHeight": "8041270",
            "blockHash": "0xf105ef7236fead0e24837769c428115f72ac58a751f730a5623b923a35a54fe1"
        },
        "signedMessages": [],
        "index": 0,
        "assetType": "BASE_ASSET"
    }
}
""";

        var deserialized = JsonSerializer.Deserialize<TransactionWebhook>(example, JsonSerializerOptions.Web);

        Console.WriteLine(deserialized);
    }
}