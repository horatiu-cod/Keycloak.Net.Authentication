using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Token;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.User.Apis.Features.User.UserRefreshToken;

internal class UserRefreshTokenQuery : IUserRefreshTokenQuery
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UserRefreshTokenQuery(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Result<TokenRepresentation?>> RefreshTokenAsync(string baseAddress, string realmName, string clientId, string clientSecret, string refreshtoken, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient("kapi");
        var tokenUrl = BaseUrl.TokenUrl(baseAddress, realmName);

        var userRefreshTokenRequest = new UserRefreshTokenRequest(clientId, clientSecret, refreshtoken);
        try
        {
            var body = UserRefreshTokenRequestBodyBuilder.UserRefreshTokenRequestBody(userRefreshTokenRequest);
            var response = await httpClient.PostAsync(tokenUrl, body, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);
                return Result<TokenRepresentation?>.Fail(response.StatusCode, (string?)content!["error_description"]);
            }
            else
            {
                var getTokenResponse = await response.Content.ReadFromJsonAsync<TokenRepresentation>(cancellationToken);
                if (response.StatusCode == HttpStatusCode.OK && getTokenResponse is not null)
                {
                    return Result<TokenRepresentation?>.Success(getTokenResponse, response.StatusCode);
                }
                else
                {
                    return Result<TokenRepresentation?>.Fail($"Refresh token not found from the Client");
                }
            }
        }
        catch (Exception ex)
        {
            return Result<TokenRepresentation?>.Fail(HttpStatusCode.InternalServerError, $"Something went wrong //br{ex.Message}");
        }
    }

}
