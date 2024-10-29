using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Configuration;
using Keycloak.Net.User.Api.Features.Client.ClientAccessToken;
using Keycloak.Net.User.Api.Features.User.Get;

namespace Keycloak.Net.User.Api.Features.User.Update;

internal class UpdateUserCommand(IHttpClientFactory httpClientFactory, IGetClientTokenQuery clientTokenQuery, IGetUserQueryInternal userQuery, IOptionsMonitor<Server> server, IOptionsMonitor<AdminClient> adminClient) : IUpdateUserCommand
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IGetClientTokenQuery _clientTokenQuery = clientTokenQuery;
    private readonly IGetUserQueryInternal _userQuery = userQuery;
    private readonly Server _server = server.CurrentValue;
    private readonly AdminClient _adminClient = adminClient.CurrentValue;

    public async Task<Result> UpdateUserAsync(string username, string? firstName, string? lastName, string? email, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient("kapi");
        var tokenUrl = BaseUrl.TokenUrl(_server.BaseAddress, _server.RealmName);
        var adminUrl = BaseUrl.AdminUrl(_server.BaseAddress, _server.RealmName);
        var client = new GetClientTokenRequest(_adminClient.ClientId, _adminClient.ClientSecret);

        var tokenResponse = await _clientTokenQuery.GetClientTokenAsync(tokenUrl, client, httpClient);
        if (!tokenResponse.IsSuccess)
            return Result.Fail(tokenResponse.StatusCode, tokenResponse.Error);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Content?.AccessToken);
        var userResponse = await _userQuery.Handler(adminUrl, username, httpClient);
        if (!userResponse.IsSuccess)
            return Result.Fail(userResponse.StatusCode, userResponse.Error);
        try
        {
            var usr = userResponse.Content;
            var user = new UpdateUserRequest
            {

                FirstName = !string.IsNullOrEmpty(firstName) ? firstName : usr?.FirstName,
                LastName = !string.IsNullOrEmpty(lastName) ? lastName : usr?.LastName,
                Email = !string.IsNullOrEmpty(email) ? email : usr?.Email,
            };

            var response = await httpClient.PutAsJsonAsync($"{adminUrl}/users/{userResponse.Content?.Id}", user, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return Result.Success(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>();
                return Result.Fail(response.StatusCode, (string?)content!["error_description"]);
            }
        }
        catch (Exception ex)
        {
            return Result.Fail(HttpStatusCode.InternalServerError, $"Something went wrong /br{ex.Message}");
        }
    }
}
