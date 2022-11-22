using BotAssistant.Infrastructure.Yandex.Token.Model;
using Jose;
using System.Security.Cryptography;

namespace BotAssistant.Infrastructure.Yandex.Token;

public sealed class YandexTokenService : IYandexTokenService
{
    private readonly IOptions<YandexAuthorizedKeyOptions> _yandexAuthorizedKeyOptions;
    private readonly HttpClient _httpClient;
    private static IAMToken? _token = null;
    private static readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    

    public YandexTokenService(IOptions<YandexAuthorizedKeyOptions> yandexAuthorizedKeyOptions, HttpClient httpClient)
    {
        _yandexAuthorizedKeyOptions = yandexAuthorizedKeyOptions;
        _httpClient = httpClient;
    }

    public async Task<IAMToken?> GetToken()
    {
        if (_token is null || _token.Value.ExpiresAt <= DateTime.Now)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                if (_token is null || _token.Value.ExpiresAt <= DateTime.Now)
                    _token = await CreateIamTokenAsync();
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
        return _token;
    }

    private string CreateJWTToken()
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var headers = new Dictionary<string, object>()
        {
            { "kid", _yandexAuthorizedKeyOptions.Value.Id }
        };

        var payload = new Dictionary<string, object>()
        {
            { "aud", _yandexAuthorizedKeyOptions.Value.TokenUrl },
            { "iss", _yandexAuthorizedKeyOptions.Value.ServiceAccountId },
            { "iat", now },
            { "exp", now + 3600 }
        };

        using var rsa = RSA.Create();
        rsa.ImportFromPem(_yandexAuthorizedKeyOptions.Value.PrivateKey?.ToCharArray());
        string encodedToken = Jose.JWT.Encode(payload, rsa, JwsAlgorithm.PS256, headers);
        return encodedToken;

    }

    private async Task<IAMToken?> CreateIamTokenAsync()
    {
        var jwtToken = CreateJWTToken();
        TokenCreateRequest tokenCreateRequest = new() { JWT = jwtToken };
        var jsonReqeust = System.Text.Json.JsonSerializer.Serialize(tokenCreateRequest);

        using var content = new StringContent(jsonReqeust);
        var reqUri = new Uri(_yandexAuthorizedKeyOptions.Value.TokenUrl);
        var response = await _httpClient.PostAsync(reqUri, content);
        var resultResponse = await response.Content.ReadAsStringAsync();
        return IAMToken.FromStingJson(resultResponse);
    }
}
