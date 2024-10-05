using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;
using System.Net.Http.Headers;

namespace Keycloak.Net.User.Apis.Tests.Integration;

public class TokenRequestFixture
{
    private readonly IGetClientTokenQuery _clientTokenRequest;
    public HttpClient _httpClient { get; set; } = new HttpClient();
    const string clientId = "auth-client";
    const string clientSecret = "JsCpqGIfQFWWO0dhUSjaNAnZGR4JhEHC";
    //const string url = "https://localhost:8843/realms/oidc/protocol/openid-connect/token"
    readonly string realmName = "oidc";
    readonly string baseAddress = "https://localhost:8843/";

    public  TokenRequestFixture()
    {
        _clientTokenRequest = new GetClientTokenQuery();
        _httpClient = GetTokenAsync(_httpClient).Result;

    }

    public async Task<HttpClient> GetTokenAsync(HttpClient httpClient)
    {
        var client = new GetClientTokenRequest(clientId, clientSecret);
        var response = await _clientTokenRequest.GetClientTokenAsync(BaseUrl.TokenUrl(baseAddress, realmName), client, _httpClient);
        if (response.IsSuccess)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Content?.AccessToken);
            
        }
        return httpClient;
    }
}
