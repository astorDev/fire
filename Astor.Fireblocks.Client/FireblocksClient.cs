using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Nist.Queries;
using Nist.Responses;

namespace Astor.Fireblocks.Client;

public partial class FireblocksUris
{
    public const string V1 = "v1";
}

public class Sender(ILogger<Sender> logger, FireblocksAuthenticator authenticator, HttpClient client)
{
    public async Task<HttpResponseMessage> SendAsync(HttpMethod method, string uri, object? requestBody = null)
    {
        var requestBodyString = requestBody == null ? null : JsonSerializer.Serialize(requestBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
        
        var fullUri = Path.Combine(client.BaseAddress!.ToString(), uri);

        var request = new HttpRequestMessage
        {
            RequestUri = new(fullUri),    
            Method = method
        };

        if (!String.IsNullOrEmpty(requestBodyString))
        {
            request.Content = new StringContent(requestBodyString, MediaTypeHeaderValue.Parse("application/json"));
        }

        authenticator.SetHttpRequestHeaders(request.Headers, uri, requestBodyString);
        
        logger.LogInformation("Sending {method} {uri} {body}", method, uri, requestBodyString);
        
        return await client.SendAsync(request);
    }
}

public partial class FireblocksClient(Sender sender, ILogger<FireblocksClient> logger)
{
    public async Task<T> GetAsync<T>(string uri, object? query = null)
    {
        var queryUri = query == null ? uri : QueryUri.From(uri, query);
        return await SendAsync<T>(HttpMethod.Get, queryUri);
    }

    public async Task<T> PostAsync<T>(string uri, object requestBody)
    {
        return await SendAsync<T>(HttpMethod.Post, uri, requestBody);
    }

    public async Task<T> SendAsync<T>(HttpMethod method, string uri, object? requestBody = null)
    {
        return await sender.SendAsync(method, uri, requestBody).Read<T>(logger, new JsonSerializerOptions{
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        });
    }
}

public class FireblocksAuthenticator(string apiKey, RsaSecurityKey privateKey)
{
    public void SetHttpRequestHeaders(HttpRequestHeaders headers, string uri, string? requestBodyString = null)
    {
        var jwt = GetJwtToken(uri, requestBodyString);
        var jwtString = new JwtSecurityTokenHandler().WriteToken(jwt);

        headers.Add("X-API-Key", apiKey);
		headers.Authorization = new("Bearer", jwtString);
    }

    public JwtSecurityToken GetJwtToken(string uri, string? requestBody = null)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(requestBody ?? String.Empty);
        var hash = sha256.ComputeHash(bytes);
        var bodyHash = BitConverter.ToString(hash).Replace("-", "").ToLower();
        
        var payload = new JwtPayload
        {
            { "uri", uri.StartsWith("/") ? uri : $"/{uri}" },
            { "nonce", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() },
            { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
            { "exp", DateTimeOffset.UtcNow.AddSeconds(25).ToUnixTimeSeconds() },
            { "sub", apiKey },
            { "bodyHash", bodyHash }
        };

        var header = new JwtHeader(new SigningCredentials(
            privateKey,
            SecurityAlgorithms.RsaSha256));

        return new(header, payload);
    }
}