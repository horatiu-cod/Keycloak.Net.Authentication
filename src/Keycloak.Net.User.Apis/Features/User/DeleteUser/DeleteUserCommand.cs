using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.User.Apis.Features.User.DeleteUser;

internal class DeleteUserCommand : IDeleteUserCommand
{
    private readonly IGetClientTokenQuery _getClientTokenQuery;
    private readonly IHttpClientFactory _httpClientFactory;


    public DeleteUserCommand(IGetClientTokenQuery getClientTokenQuery, IHttpClientFactory httpClientFactory)
    {
        _getClientTokenQuery = getClientTokenQuery;
        _httpClientFactory = httpClientFactory;
    }
    public async Task<Result> DeleteUserAsync(string baseAddress, string realmName, string userId, string realmAdminClientId, string realmAdminClientSecret, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient("kapi");
        var tokenUrl = BaseUrl.TokenUrl(baseAddress, realmName);
        var client = new GetClientTokenRequest(realmAdminClientId, realmAdminClientSecret);
        var tokenResponse = await _getClientTokenQuery.GetClientTokenAsync(tokenUrl, client, httpClient);
        if (!tokenResponse.IsSuccess)
            return Result.Fail(tokenResponse.StatusCode, tokenResponse.Error);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Content?.AccessToken);
        try
        {
            var adminUrl = BaseUrl.AdminUrl(baseAddress, realmName);
            var response = await httpClient.DeleteAsync($"/{adminUrl}/users/{userId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>();
                return Result.Fail(response.StatusCode, (string?)content!["error_description"]);
            }
            else
            {
                return Result.Success(response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            return Result.Fail(HttpStatusCode.InternalServerError, $"Something went wrong //br{ex.Message}");
        }
    }
}

