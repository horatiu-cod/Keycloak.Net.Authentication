﻿namespace Keycloak.Net.User.Api.Features.User.Logout;

internal class LogoutUserCommand(IHttpClientFactory httpClientFactory, IOptionsMonitor<Server> server) : ILogoutUserCommand
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly Server _server = server.CurrentValue;

    public async Task<Result> LogoutUserAsync(string baseAddress, string realmName, string clientId, string clientSecret, string refreshToken, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient("kapi");
        var tokenUrl = BaseUrl.LogoutUrl(_server.BaseAddress, _server.RealmName);

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
    public async Task<Result> LogoutUserAsync(string clientId, string refreshToken, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient("kapi");
        var tokenUrl = BaseUrl.LogoutUrl(_server.BaseAddress, _server.RealmName);
        var clientSecret = string.Empty;

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
