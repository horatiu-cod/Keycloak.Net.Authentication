using System.Net.Http.Json;
using System.Net;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Options;
using Keycloak.Net.User.Api.Configuration;
using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Features.Token;

namespace Keycloak.Net.User.Api.Features.User.Login;

internal class GetUserTokenQuery(IHttpClientFactory httpClientFactory, IOptionsMonitor<Server> server) : IGetUserTokenQuery
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly Server _server = server.CurrentValue;

    public async Task<Result<TokenRepresentation?>> LoginUserAsync(string clientId, string clientSecret, string userName, string password, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient("kapi");
        var tokenUrl = BaseUrl.TokenUrl(_server.BaseAddress, _server.RealmName);

        var userRequestDto = new GetUserTokenRequest(clientId, clientSecret, userName, password);
        var requestBody = GetUserTokenRequestBodyBuilder.UserTokenRequestBody(userRequestDto);
        try
        {
            var response = await httpClient.PostAsync(tokenUrl, requestBody, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>();
                return Result<TokenRepresentation>.Fail(response.StatusCode, (string?)content!["error_description"]);
            }
            else
            {
                var token = await response.Content.ReadFromJsonAsync<TokenRepresentation>(cancellationToken);
                return Result<TokenRepresentation?>.Success(token, response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            return Result<TokenRepresentation>.Fail(HttpStatusCode.InternalServerError, $"Something went wrong //br{ex.Message}");
        }
    }

    public async Task<Result<TokenRepresentation?>> LoginUserAsync(string clientId, string userName, string password, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient("kapi");
        var tokenUrl = BaseUrl.TokenUrl(_server.BaseAddress, _server.RealmName);
        var clientSecret = string.Empty;

        var userRequestDto = new GetUserTokenRequest(clientId, clientSecret, userName, password);
        var requestBody = GetUserTokenRequestBodyBuilder.UserTokenRequestBody(userRequestDto);
        try
        {
            var response = await httpClient.PostAsync(tokenUrl, requestBody, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>();
                return Result<TokenRepresentation>.Fail(response.StatusCode, (string?)content!["error_description"]);
            }
            else
            {
                var token = await response.Content.ReadFromJsonAsync<TokenRepresentation>(cancellationToken);
                return Result<TokenRepresentation?>.Success(token, response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            return Result<TokenRepresentation>.Fail(HttpStatusCode.InternalServerError, $"Something went wrong //br{ex.Message}");
        }
    }
}
