﻿namespace Keycloak.Net.User.Apis.Features.User.Register;

internal class RegisterUserWithRealmRoleCommand(IGetClientTokenQuery getClientTokenQuery, IGetRealmRoleQuery getRealmRoleQuery, IAssignRealmRoleInternalCommand userRoleCommand, IGetUserIdQuery userRequest, IHttpClientFactory httpClientFactory, IOptionsMonitor<Server> server, IOptionsMonitor<AdminClient> adminClient) : IRegisterUserWithRealmRoleCommand
{
    private readonly IGetClientTokenQuery _getClientTokenQuery = getClientTokenQuery;
    private readonly IGetRealmRoleQuery _getRealmRoleQuery = getRealmRoleQuery;
    private readonly IAssignRealmRoleInternalCommand _userRoleCommand = userRoleCommand;
    private readonly IGetUserIdQuery _userRequest = userRequest;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly Server _server = server.CurrentValue;
    private readonly AdminClient _adminClient = adminClient.CurrentValue;

    public async Task<Result> RegisterUserWithRealmRole(string username, string password, string email, string firstName, string lastName, string[] rolesName, CancellationToken cancellationToken)
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
        var userResponse = await _userRequest.GetUserIdAsync(adminUrl, username, httpClient);
        if (!userResponse.IsSuccess)
            return Result.Fail(userResponse.StatusCode, userResponse.Error);
        var roles = new List<RoleRepresentation>();
        foreach (var role in rolesName)
        {
            var response = await _getRealmRoleQuery.GetRealmRoleAsync(adminUrl, role, httpClient, cancellationToken);
            if (!response.IsSuccess)
                return Result.Fail(response.StatusCode, response.Error);
            if (response.IsSuccess && response.Content != null)
                roles.Add(response.Content);
        }
        var result = await _userRoleCommand.AssignRealmRolesAsync(adminUrl, userResponse.Content!, roles.ToArray(), httpClient, cancellationToken);
        if (!result.IsSuccess)
            return Result.Fail(result.StatusCode, result.Error);
        return Result.Success(HttpStatusCode.Created);
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