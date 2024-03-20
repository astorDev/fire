using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nist.Responses;

namespace Astor.Fireblocks.Client;

public partial class FireblocksUris
{
    public const string Vault = "vault";
    public const string AccountsPaged = "accounts_paged";
    public static string VaultAccountsPaged = $"{Vault}/{AccountsPaged}";
}

public partial class FireblocksClient(HttpClient client, FireblocksAuthenticator authenticator)
{
    public async Task<object> GetAccountsPaged()
    {
        return await GetAsync<object>(FireblocksUris.VaultAccountsPaged);
    }

    public async Task<T> GetAsync<T>(string uri)
    {
        return await SendAsync<T>(HttpMethod.Get, uri);
    }

    public async Task<T> SendAsync<T>(HttpMethod method, string uri, object? requestBody = null)
    {
        var requestBodyString = JsonSerializer.Serialize(requestBody);

        var fullUri = client.BaseAddress?.ToString() ?? "" + uri;

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(fullUri),    
            Method = method,
            Content = new StringContent(requestBodyString)
        };

        authenticator.SetHttpRequestHeaders(request.Headers, uri, requestBodyString);
        return await client.SendAsync(request).Read<T>();
    }
}

public class FireblocksAuthenticator(IOptions<FireblocksClientOptions> options)
{
    public void SetHttpRequestHeaders(HttpRequestHeaders headers, string uri, string? requestBodyString = null)
    {
        var jwt = GetJwtToken(uri, requestBodyString);
        var jwtString = new JwtSecurityTokenHandler().WriteToken(jwt);

        headers.Add("X-API-Key", options.Value.ApiKey);
		headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtString);
    }

    public JwtSecurityToken GetJwtToken(string uri, string? requestBody = null)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(requestBody ?? "\"\""));
        var body = BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();

        using RSA rsa = RSA.Create();
        rsa.ImportPkcs8PrivateKey(options.Value.ApiSecretBytes, out _);

        return new JwtSecurityToken(
            claims: [
                new Claim("uri", uri),
                new Claim("nonce", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddSeconds(25).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Sub, options.Value.ApiKey),
                new Claim("bodyHash", body)
            ],
            signingCredentials: new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
            {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            }
        );
    }
}