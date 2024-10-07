using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;
using Keycloak.Net.User.Apis.Features.Role;
using Keycloak.Net.User.Apis.Features.Role.ClientRole;
using Keycloak.Net.User.Apis.Features.Role.RealmRole;
using Keycloak.Net.User.Apis.Features.Role.UserRole;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.User.Apis.Features.User.RegisterUser;

internal class RegisterUserCommand : IRegisterUserCommand
{
    private readonly IGetClientTokenQuery _getClientTokenQuery;
    private readonly IGetRealmRoleQuery _getRealmRoleQuery;
    private readonly IAssignUserRoleCommand _userRoleCommand;
    private readonly IGetClientRoleQuery _getClientRoleQuery;
    private readonly IGetUserQuery _userRequest;
    private readonly IHttpClientFactory _httpClient;
    public RegisterUserCommand(IGetClientTokenQuery getClientTokenQuery, IGetRealmRoleQuery getRealmRoleQuery, IAssignUserRoleCommand userRoleCommand, IGetClientRoleQuery getClientRoleQuery, IGetUserQuery userRequest, IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
        _getClientTokenQuery = getClientTokenQuery;
        _getRealmRoleQuery = getRealmRoleQuery;
        _userRoleCommand = userRoleCommand;
        _getClientRoleQuery = getClientRoleQuery;
        _userRequest = userRequest;
    }

    public async Task<Result> RegisterUserWithRealmRole(string baseAddress, string realmName, string clientId, string clientSecret, string username, string password, string[] rolesName)
    {
        var httpClient = _httpClient.CreateClient("kapi");
        var tokenUrl = BaseUrl.TokenUrl(baseAddress, realmName);
        var adminUrl = BaseUrl.AdminUrl(baseAddress, realmName);
        var client = new GetClientTokenRequest(clientId, clientSecret);
        var tokenResponse = await _getClientTokenQuery.GetClientTokenAsync(tokenUrl, client, httpClient);
        if(!tokenResponse.IsSuccess)
            return Result.Fail(tokenResponse.StatusCode, tokenResponse.Error);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Content!.AccessToken!);

        var registerResponse = await Register(adminUrl, username, password,  httpClient);
        if(!registerResponse.IsSuccess)
            return Result.Fail(registerResponse.StatusCode, registerResponse.Error);
        var userResponse = await _userRequest.GetUserIdAsync(adminUrl, username, httpClient);
        if(!userResponse.IsSuccess)
            return Result.Fail(userResponse.StatusCode, userResponse.Error);
        var roles = new List<RoleRepresentation>();
        foreach (var role in rolesName)
        {
            var response = await _getRealmRoleQuery.GetRealmRoleAsync(adminUrl, role, httpClient);
            if(!response.IsSuccess)
                return Result.Fail(response.StatusCode, response.Error);
            if(response.IsSuccess && response.Content != null)
                roles.Add(response.Content);
        }
        var result = await _userRoleCommand.AssignRealmRolesToUserAsync(adminUrl, userResponse.Content!, roles.ToArray(),  httpClient);
        if (!result.IsSuccess)
            return Result.Fail(result.StatusCode, result.Error);
        return Result.Success(System.Net.HttpStatusCode.Created);
    }
    public async Task<Result> RegisterUserWithClientRole(string baseAddress, string realmName, string clientId, string clientSecret, string clientUuid, string username, string password, string[] rolesName)
    {
        var httpClient = _httpClient.CreateClient("kapi");
        var tokenUrl = BaseUrl.TokenUrl(baseAddress, realmName);
        var adminUrl = BaseUrl.AdminUrl(baseAddress, realmName);
        var client = new GetClientTokenRequest(clientId, clientSecret);

        var tokenResponse = await _getClientTokenQuery.GetClientTokenAsync(tokenUrl, client, httpClient);
        if (!tokenResponse.IsSuccess && tokenResponse.Content is null)
            return Result.Fail(tokenResponse.StatusCode, tokenResponse.Error);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Content!.AccessToken!);

        var registerResponse = await Register(adminUrl, username, password, httpClient);
        if (!registerResponse.IsSuccess)
            return Result.Fail(registerResponse.StatusCode, registerResponse.Error);

        var userResponse = await _userRequest.GetUserIdAsync(adminUrl, username, httpClient);
        if (!userResponse.IsSuccess)
            return Result.Fail(userResponse.StatusCode, userResponse.Error);

        var roles = new List<RoleRepresentation>();
        foreach (var role in rolesName)
        {
            var response = await _getClientRoleQuery.GetClientRoleAsync(adminUrl, clientUuid, role, httpClient);
            if (!response.IsSuccess)
                return Result.Fail(response.StatusCode, response.Error);
            if (response.IsSuccess && response.Content != null)
                roles.Add(response.Content);
        }
        var result = await _userRoleCommand.AssignRealmRolesToUserAsync(adminUrl, userResponse.Content!, roles.ToArray(), httpClient);
        if (!result.IsSuccess)
            return Result.Fail(result.StatusCode, result.Error);
        return Result.Success(result.StatusCode);
    }
    public async Task<Result> RegisterUser(string baseAddress, string realmName, string clientId, string clientSecret, string username, string password)
    {
        var httpClient = _httpClient.CreateClient("kapi");
        var tokenUrl = BaseUrl.TokenUrl(baseAddress, realmName);
        var adminUrl = BaseUrl.AdminUrl(baseAddress, realmName);
        var client = new GetClientTokenRequest(clientId, clientSecret);

        var tokenResponse = await _getClientTokenQuery.GetClientTokenAsync(tokenUrl, client, httpClient);
        if (!tokenResponse.IsSuccess)
            return Result.Fail(tokenResponse.StatusCode, tokenResponse.Error);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Content!.AccessToken!);

        var registerResponse = await Register(adminUrl, username, password, httpClient);
        if (!registerResponse.IsSuccess)
            return Result.Fail(registerResponse.StatusCode, registerResponse.Error);
        return Result.Success(registerResponse.StatusCode);
    }

    private async Task<Result> Register(string url, string username, string password,  HttpClient httpClient)
    {
        var credentials = new Credentials(password);
        var user = new RegisterUserRequest()
        {
            Email = username,
            UserName = username,
            Credentials = [credentials]
        };
        try
        {
            url = $"{url}/users";
            var response = await httpClient.PostAsJsonAsync(url, user);
            if(response.StatusCode == System.Net.HttpStatusCode.Created)
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
