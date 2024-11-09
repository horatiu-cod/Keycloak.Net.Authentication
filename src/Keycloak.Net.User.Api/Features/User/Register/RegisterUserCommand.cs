namespace Keycloak.Net.User.Api.Features.User.Register;

internal class RegisterUserCommand(IGetClientTokenQuery getClientTokenQuery, IHttpClientFactory httpClientFactory, IOptionsMonitor<Server> server, IOptionsMonitor<AdminClient> adminClient) : IRegisterUserCommand
{
    private readonly IGetClientTokenQuery _getClientTokenQuery = getClientTokenQuery;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly Server _server = server.CurrentValue;
    private readonly AdminClient _adminClient = adminClient.CurrentValue;

    public async Task<Result> Handler(string username, string password, string email, string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient("kapi");
        var tokenUrl = BaseUrl.TokenUrl(_server.BaseAddress, _server.RealmName);
        var adminUrl = BaseUrl.AdminUrl(_server.BaseAddress, _server.RealmName);
        var client = new GetClientTokenRequest(_adminClient.ClientId, _adminClient.ClientSecret);

        var tokenResponse = await _getClientTokenQuery.GetClientTokenAsync(tokenUrl, client, httpClient, cancellationToken);
        if (!tokenResponse.IsSuccess)
            return Result.Fail(tokenResponse.StatusCode, tokenResponse.Error);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Content!.AccessToken!);

        var registerUser = new RegisterUserRequest
        {
            UserName = username,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Credentials = [new Credentials(password)]
        };

        var registerResponse = await Register(adminUrl, registerUser, httpClient, cancellationToken);
        if (!registerResponse.IsSuccess)
            return Result.Fail(registerResponse.StatusCode, registerResponse.Error);
        return Result.Success(registerResponse.StatusCode);
    }

    private static async Task<Result> Register(string url, RegisterUserRequest registerUserRequest, HttpClient httpClient, CancellationToken cancellationToken)
    {
        try
        {
            url = $"{url}/users";
            var response = await httpClient.PostAsJsonAsync(url, registerUserRequest, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Created)
                return Result.Success(response.StatusCode);
            var content = await response.Content.ReadFromJsonAsync<JsonObject>();
            return Result.Fail(response.StatusCode, (string?)content?["ErrorMessage"]);
        }
        catch (Exception ex)
        {
            return Result.Fail(HttpStatusCode.InternalServerError, $"Something went wrong /br{ex.Message}");
        }
    }
}
