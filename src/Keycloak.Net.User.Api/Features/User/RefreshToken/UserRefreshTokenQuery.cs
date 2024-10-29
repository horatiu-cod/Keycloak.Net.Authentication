using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Configuration;
using Keycloak.Net.User.Api.Features.Token;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.User.Api.Features.User.RefreshToken;

internal class UserRefreshTokenQuery(IHttpClientFactory httpClientFactory, IOptionsMonitor<Server> server) : IUserRefreshTokenQuery
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly Server _server = server.CurrentValue;

    public async Task<Result<TokenRepresentation?>> RefreshTokenAsync(string clientId, string clientSecret, string refreshToken, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient("kapi");
        var tokenUrl = BaseUrl.TokenUrl(_server.BaseAddress, _server.RealmName);

        var userRefreshTokenRequest = new UserRefreshTokenRequest(clientId, clientSecret, refreshToken);
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
    public async Task<Result<TokenRepresentation?>> RefreshTokenAsync(string clientId, string refreshToken, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient("kapi");
        var tokenUrl = BaseUrl.TokenUrl(_server.BaseAddress, _server.RealmName);
        var clientSecret = string.Empty;
        var userRefreshTokenRequest = new UserRefreshTokenRequest(clientId, clientSecret, refreshToken);
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
