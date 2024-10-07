using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.User.Apis.Features.User.UpdateUser;

internal class UpdateUserCommand : IUpdateUserCommand
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IGetClientTokenQuery _clientTokenQuery;
    private readonly IGetUserQuery _userQuery;

    public UpdateUserCommand(IHttpClientFactory httpClientFactory, IGetClientTokenQuery clientTokenQuery, IGetUserQuery userQuery)
    {
        _httpClientFactory = httpClientFactory;
        _clientTokenQuery = clientTokenQuery;
        _userQuery = userQuery;
    }

    public async Task<Result> UpdateUserAsync(string baseAddress, string realmName, string clientId, string clientSecret, string username, string? firstName, string? lastName, string? email, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient("kapi");
        var tokenUrl = BaseUrl.TokenUrl(baseAddress, realmName);
        var adminUrl = BaseUrl.AdminUrl(baseAddress, realmName);
        var client = new GetClientTokenRequest(clientId, clientSecret);

        var tokenResponse = await _clientTokenQuery.GetClientTokenAsync(tokenUrl, client, httpClient);
        if (!tokenResponse.IsSuccess)
            return Result.Fail(tokenResponse.StatusCode, tokenResponse.Error);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Content?.AccessToken);
        var userResponse = await _userQuery.GetUserAsync(adminUrl, username, httpClient);
        if (!userResponse.IsSuccess)
            return Result.Fail(userResponse.StatusCode, userResponse.Error);
        try
        {
            var usr = userResponse.Content;
            var user = new UpdateUserRequest
            {
                FirstName = !string.IsNullOrEmpty( firstName)? firstName: usr?.FirstName,
                LastName = !string.IsNullOrEmpty(lastName)? lastName : usr?.LastName,
                Email = !string.IsNullOrEmpty(email)? email: usr?.Email,
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
