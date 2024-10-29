using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Configuration;
using Keycloak.Net.User.Api.Features.Client.ClientAccessToken;
using Keycloak.Net.User.Api.Features.User.Get;

namespace Keycloak.Net.User.Api.Features.User.Delete;

internal class DeleteUserCommand(IGetClientTokenQuery getClientTokenQuery, IHttpClientFactory httpClientFactory, IGetUserIdQuery userQuery, IOptionsMonitor<Server> server, IOptionsMonitor<AdminClient> adminClient) : IDeleteUserCommand
{
    private readonly IGetClientTokenQuery _getClientTokenQuery = getClientTokenQuery;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IGetUserIdQuery _userQuery = userQuery;
    private readonly Server _server = server.CurrentValue;
    private readonly AdminClient _adminClient = adminClient.CurrentValue;

    public async Task<Result> Handler(string username, string realmAdminClientId, string realmAdminClientSecret, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient("kapi");
        var tokenUrl = BaseUrl.TokenUrl(_server.BaseAddress, _server.RealmName);
        var adminUrl = BaseUrl.AdminUrl(_server.BaseAddress, _server.RealmName);

        var client = new GetClientTokenRequest(_adminClient.ClientId, _adminClient.ClientSecret);
        var tokenResponse = await _getClientTokenQuery.GetClientTokenAsync(tokenUrl, client, httpClient);
        if (!tokenResponse.IsSuccess)
            return Result.Fail(tokenResponse.StatusCode, tokenResponse.Error);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Content?.AccessToken);
        var userResponse = await _userQuery.GetUserIdAsync(adminUrl, username, httpClient);
        if (!userResponse.IsSuccess)
            return Result.Fail(userResponse.StatusCode, userResponse.Error);
        try
        {
            var response = await httpClient.DeleteAsync($"{adminUrl}/users/{userResponse.Content}", cancellationToken);
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

