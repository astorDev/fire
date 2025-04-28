using System.Net;
using System.Text.Json;
using Nist.Responses;

namespace Fire.Blocks;

public record Error(
    string Message,
    int Code
);

public class FireblocksErrorOccurredException(HttpStatusCode statusCode, Error error) : Exception()
{
    public HttpStatusCode StatusCode { get; } = statusCode;
    public Error Error { get; } = error;

    public override string Message => $"{base.Message} ({StatusCode}) {Error.Code}: {Error.Message}";

    public static FireblocksErrorOccurredException? From(UnsuccessfulResponseException ure)
    {
        var error = ure.DeserializedBody<Error>(JsonSerializerOptions.Web);
        return new FireblocksErrorOccurredException(ure.StatusCode, error!);
    }
}