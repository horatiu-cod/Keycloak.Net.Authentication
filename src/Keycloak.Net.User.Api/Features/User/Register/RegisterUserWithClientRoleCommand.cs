using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Configuration;
using Keycloak.Net.User.Api.Features.Client.ClientAccessToken;
using Keycloak.Net.User.Api.Features.Client.ClientRequest;
using Keycloak.Net.User.Api.Features.Role;
using Keycloak.Net.User.Api.Features.Role.ClientRole;
using Keycloak.Net.User.Api.Features.Role.UserRole;
using Keycloak.Net.User.Api.Features.User.Get;

namespace Keycloak.Net.User.Api.Features.User.Register;

internal class RegisterUserWithClientRoleCommand(IGetClientTokenQuery getClientTokenQuery, IGetClientIdQuery getClientIdQuery, IAssignClientRoleInternalCommand userRoleCommand, IGetClientRoleQuery getClientRoleQuery, IGetUserIdQuery userRequest, IHttpClientFactory httpClientFactory, IOptionsMonitor<Server> server, IOptionsMonitor<AdminClient> adminClient) : IRegisterUserWithClientRoleCommand
{
    private readonly IGetClientTokenQuery _getClientTokenQuery = getClientTokenQuery;
    private readonly IGetClientIdQuery _getClientIdQuery = getClientIdQuery;
    private readonly IAssignClientRoleInternalCommand _userRoleCommand = userRoleCommand;
    private readonly IGetClientRoleQuery _getClientRoleQuery = getClientRoleQuery;
    private readonly IGetUserIdQuery _userRequest = userRequest;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly AdminClient _adminClient = adminClient.CurrentValue;
    private readonly Server _server = server.CurrentValue;

    public async Task<Result> Handler(string clientId, string username, string password, string email, string firstName, string lastName, string[] rolesName, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient("kapi");
        var tokenUrl = BaseUrl.TokenUrl(_server.BaseAddress, _server.RealmName);
        var adminUrl = BaseUrl.AdminUrl(_server.BaseAddress, _server.RealmName);
        var client = new GetClientTokenRequest(_adminClient.ClientId, _adminClient.ClientSecret);

        var tokenResponse = await _getClientTokenQuery.GetClientTokenAsync(tokenUrl, client, httpClient);
        if (!tokenResponse.IsSuccess && tokenResponse.Content is null)
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
        var registerResponse = await Register(adminUrl, registerUser, httpClient);
        if (!registerResponse.IsSuccess)
            return Result.Fail(registerResponse.StatusCode, registerResponse.Error);

        var userResponse = await _userRequest.GetUserIdAsync(adminUrl, username, httpClient);
        if (!userResponse.IsSuccess)
            return Result.Fail(userResponse.StatusCode, userResponse.Error);

        var clientIdResponse = await _getClientIdQuery.GetClientIdAsync(adminUrl, clientId, httpClient, cancellationToken);
        if (!clientIdResponse.IsSuccess)
            return Result.Fail(clientIdResponse.StatusCode, clientIdResponse.Error);


        var roles = new List<RoleRepresentation>();
        foreach (var role in rolesName)
        {
            var response = await _getClientRoleQuery.GetClientRoleAsync(adminUrl, clientIdResponse.Content!.Id, role, httpClient);
            if (!response.IsSuccess)
                return Result.Fail(response.StatusCode, response.Error);
            if (response.IsSuccess && response.Content != null)
                roles.Add(response.Content);
        }
        var result = await _userRoleCommand.AssignClientRolesAsync(adminUrl, userResponse.Content!, clientIdResponse.Content!.Id, roles.ToArray(), httpClient);
        if (!result.IsSuccess)
            return Result.Fail(result.StatusCode, result.Error);
        return Result.Success(registerResponse.StatusCode);
    }
    private static async Task<Result> Register(string url, RegisterUserRequest registerUserRequest, HttpClient httpClient)
    {
        try
        {
            url = $"{url}/users";
            var response = await httpClient.PostAsJsonAsync(url, registerUserRequest);
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
