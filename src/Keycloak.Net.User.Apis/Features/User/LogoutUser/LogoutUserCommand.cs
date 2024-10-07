using Keycloak.Net.User.Apis.Common;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.User.Apis.Features.User.LogoutUser;

internal class LogoutUserCommand : ILogoutUserCommand
{
    private readonly IHttpClientFactory _httpClientFactory;

    public LogoutUserCommand(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Result> LogoutUserAsync(string baseAddress, string realmName, string clientId, string clientSecret, string refreshToken, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient("kapi");
        var tokenUrl = BaseUrl.LogoutUrl(baseAddress, realmName);

        var logoutUserRequestDto = new LogoutUserRequest(clientId, clientSecret, refreshToken);
        var requestBody = LogoutUserRequestBodyBuilder.LogoutUserTokenRequestBody(logoutUserRequestDto);
        try
        {
            var response = await httpClient.PostAsync(tokenUrl, requestBody, cancellationToken);
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
            return Result.Fail(HttpStatusCode.InternalServerError, $"Something went wrong //br{ex.Message}");
        }
    }
}
