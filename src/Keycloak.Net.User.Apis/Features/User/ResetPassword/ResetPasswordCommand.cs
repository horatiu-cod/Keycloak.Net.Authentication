using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.User.Apis.Features.User.ResetPassword;

internal class ResetPasswordCommand : IResetPasswordCommand
{
    private readonly IGetClientTokenQuery _getClientTokenQuery;
    private readonly IHttpClientFactory _httpClientFactory;

    public ResetPasswordCommand(IGetClientTokenQuery getClientTokenQuery, IHttpClientFactory httpClientFactory)
    {
        _getClientTokenQuery = getClientTokenQuery;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Result> ResetPasswordAsync(string baseAddress, string realmName, string clientId, string clientSecret, string userId, string password, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var tokenUrl = BaseUrl.TokenUrl(baseAddress, realmName);
        var adminUrl = BaseUrl.RealmUrl(baseAddress, realmName);
        var client = new GetClientTokenRequest(clientId, clientSecret);

        var tokenResponse = await _getClientTokenQuery.GetClientTokenAsync(tokenUrl, client, httpClient);
        if (!tokenResponse.IsSuccess)
            return Result.Fail(tokenResponse.StatusCode, tokenResponse.Error);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Content?.AccessToken);

        var passwordRequest = new ResetPasswordRequest(password);
        try
        {
            var response = await httpClient.PutAsJsonAsync<ResetPasswordRequest>($"{adminUrl}/users/{userId}/reset-password", passwordRequest, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);
                return Result.Fail(response.StatusCode, (string?)content!["error_description"]);
            }
            else
            {
                return Result.Success(response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            return Result.Fail(HttpStatusCode.InternalServerError, $"Something went wrong /br{ex.Message}");
        }
    }
}
