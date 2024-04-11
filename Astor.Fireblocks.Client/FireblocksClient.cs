using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nist.Queries;
using Nist.Responses;

namespace Astor.Fireblocks.Client;

public partial class FireblocksUris
{
    public const string V1 = "v1";
}

public class Sender
{
    readonly ILogger<Sender> logger;
    readonly FireblocksAuthenticator authenticator;
    readonly HttpClient client;

    public Sender(ILogger<Sender> logger, FireblocksAuthenticator authenticator, HttpClient client)
    {
        this.logger = logger;
        this.authenticator = authenticator;
        this.client = client;
    }
    
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

public partial class FireblocksClient
{
    readonly Sender sender;
    readonly ILogger<FireblocksClient> logger;
    
    public FireblocksClient(Sender sender, ILogger<FireblocksClient> logger)
    {
        this.sender = sender;
        this.logger = logger;
    }
    
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
        return await sender.SendAsync(method, uri, requestBody).Read<T>(logger);
    }
}

public class FireblocksAuthenticator
{
    readonly IOptions<FireblocksClientOptions> options;
    
    public FireblocksAuthenticator(IOptions<FireblocksClientOptions> options)
    {
        this.options = options;
    }
    
    public void SetHttpRequestHeaders(HttpRequestHeaders headers, string uri, string? requestBodyString = null)
    {
        var jwt = GetJwtToken(uri, requestBodyString);
        var jwtString = new JwtSecurityTokenHandler().WriteToken(jwt);

        headers.Add("X-API-Key", options.Value.ApiKey);
		headers.Authorization = new("Bearer", jwtString);
    }

    public JwtSecurityToken GetJwtToken(string uri, string? requestBody = null)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(requestBody ?? String.Empty);
        var hash = sha256.ComputeHash(bytes);
        var bodyHash = BitConverter.ToString(hash).Replace("-", "").ToLower();
        
        var rsa = RSA.Create();
        rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(options.Value.ApiSecret), out _);

        var payload = new JwtPayload
        {
            { "uri", $"/{uri}" },
            { "nonce", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() },
            { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
            { "exp", DateTimeOffset.UtcNow.AddSeconds(25).ToUnixTimeSeconds() },
            { "sub", options.Value.ApiKey },
            { "bodyHash", bodyHash }
        };
        
        return new(new(new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)), payload);
    }
}