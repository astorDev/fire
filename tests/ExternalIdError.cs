using System.Net;
using Nist.Responses;

namespace Fire.Blocks;

[TestClass]
public class ExternalIdError : Test
{
    [TestMethod]
    public async Task HandleError()
    {
        var candidate = new TransactionCandidate(
            AssetId: "ETH_TEST5",
            Amount: 0.003m,
            Source: new(
                Type: PeerTypes.VaultAccount,
                Id: Environment.GetEnvironmentVariable("HOT_WALLET_ID")!
            ),
            Destination: new TransactionDestination(
                Type: PeerTypes.OneTimeAddress,
                OneTimeAddress: new OneTimeAddress(
                    Address: "0x6a39933f2968490686Dfa8f70a2398F620df054D"
                )
            ),
            Note: "",
            ExternalTxId: "idempotency-666"
        );

        try {
            var result = await Client.PostTransaction(candidate);
        }
        catch (FireblocksErrorOccurredException ex) {
            ex.Error.Code.ShouldBe(ErrorCodes.ExternalTxIdDuplicate);
        }
    }

    [TestMethod]
    public void MapErrors()
    {
        var body = """{"message":"The external tx id that was provided in the request, already exists","code":1438}""";
        var ex = new UnsuccessfulResponseException(
            HttpStatusCode.BadRequest, 
            null!,
            body: body
        );

        FireblocksErrorOccurredException.From(ex)!.Error.Code.ShouldBe(ErrorCodes.ExternalTxIdDuplicate);
    }
}