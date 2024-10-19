namespace Keycloak.Net.User.Apis.Features.User.ResetPassword;

internal class ResetPasswordCommand(IGetClientTokenQuery getClientTokenQuery, IHttpClientFactory httpClientFactory, IGetUserIdQuery userQuery, IOptionsMonitor<Server> server, IOptionsMonitor<AdminClient> adminClient) : IResetPasswordCommand
{
    private readonly IGetClientTokenQuery _getClientTokenQuery = getClientTokenQuery;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IGetUserIdQuery _userIdQuery = userQuery;
    private readonly Server _server = server.CurrentValue;
    private readonly AdminClient _adminClient = adminClient.CurrentValue;

    public async Task<Result> Handler(string username, string password, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient("kapi");
        var tokenUrl = BaseUrl.TokenUrl(_server.BaseAddress, _server.RealmName);
        var adminUrl = BaseUrl.AdminUrl(_server.BaseAddress, _server.RealmName);
        var client = new GetClientTokenRequest(_adminClient.ClientId, _adminClient.ClientSecret);

        var tokenResponse = await _getClientTokenQuery.GetClientTokenAsync(tokenUrl, client, httpClient, cancellationToken);
        if (!tokenResponse.IsSuccess)
            return Result.Fail(tokenResponse.StatusCode, tokenResponse.Error);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Content?.AccessToken);
        var userResponse = await _userIdQuery.GetUserIdAsync(adminUrl, username, httpClient, cancellationToken);
        if (!userResponse.IsSuccess)
            return Result.Fail(userResponse.StatusCode, userResponse.Error);
        var credentials = new Credentials(password);
        //var passwordRequest = new ResetPasswordRequest
        //{
        //    Credentials  = [credentials],
        //};
        try
        {
            var response = await httpClient.PutAsJsonAsync($"{adminUrl}/users/{userResponse.Content}/reset-password", credentials, cancellationToken);
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
