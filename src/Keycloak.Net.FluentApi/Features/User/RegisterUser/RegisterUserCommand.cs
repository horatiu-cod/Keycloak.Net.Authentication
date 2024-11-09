using Keycloak.Net.FluentApi.Common;
using Keycloak.Net.FluentApi.Features.Client.ClientAccessToken;

namespace Keycloak.Net.FluentApi.Features.User.RegisterUser;

internal class RegisterUserCommand : IRegisterUserCommand
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IClientTokenRequest _clientTokenRequest;

    public RegisterUserCommand(IClientTokenRequest clientTokenRequest, IHttpClientFactory httpClientFactory)
    {
        _clientTokenRequest = clientTokenRequest;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Result> RegisterUser(string url, string uri, string clientId, string clientSecret, string username, string password, string email, string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient();
        ArgumentNullException.ThrowIfNull(nameof(httpClient));

        var tokenResponse = await _clientTokenRequest.GetClientTokenAsync(uri, clientId, clientSecret, httpClient!, cancellationToken);
        if (!tokenResponse.IsSuccess && String.IsNullOrEmpty(tokenResponse.Content))
            return Result.Fail(tokenResponse.StatusCode, tokenResponse.Error);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Content);

        var registerResult = await Register(username, password, email, firstName, lastName, url, httpClient!, cancellationToken);
        if (!registerResult.IsSuccess)
            return Result.Fail(registerResult.StatusCode, registerResult.Error);
        return Result.Success(registerResult.StatusCode);
    }

    private static async Task<Result> Register(string username, string password, string email, string firstName, string lastName, string url, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        var user = new RegisterUserRequest
        {
            UserName = username,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Credentials = [new Credentials(password)]
        };
        url = $"{url}/users";
        try
        {
            var response = await httpClient.PostAsJsonAsync(url, user, cancellationToken);
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
