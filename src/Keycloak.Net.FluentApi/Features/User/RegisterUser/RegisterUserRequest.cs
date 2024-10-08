﻿using Keycloak.Net.FluentApi.Common;
using Keycloak.Net.FluentApi.Features.Client;
using Keycloak.Net.FluentApi.Features.Client.ClientAccessToken;
using Keycloak.Net.FluentApi.Features.Role.ClientRole;
using Keycloak.Net.FluentApi.Features.Role.RealmRole;
using Keycloak.Net.FluentApi.Features.User.UserRole;

namespace Keycloak.Net.FluentApi.Features.User.RegisterUser;

internal class RegisterUserRequest : IRegisterUserRequest
{
    private readonly IHttpClientFactory? _httpClientFactory;
    private IClientTokenRequest ClientTokenRequest { get; }
    private IRealmRoleRequest RealmRoleRequest { get; }
    private IClientRoleRequest ClientRoleRequest { get; }
    private IUserRealmRoleRequest UserRealmRoleRequest { get; }
    private IUserRequest UserRequest { get; }
    private IClientRequest ClientRequest { get; }
    public RegisterUserRequest()
    {
        ClientTokenRequest = new ClientTokenRequest();
        RealmRoleRequest = new RealmRoleRequest();
        UserRealmRoleRequest = new UserRealmRoleRequest();
        UserRequest = new UserRequest();
        ClientRequest = new ClientRequest();
        ClientRoleRequest = new ClientRoleRequest();
    }

    public RegisterUserRequest(IClientTokenRequest clientTokenRequest, IRealmRoleRequest realmRoleRequest, IUserRealmRoleRequest userRoleRequest, IUserRequest userRequest, IClientRequest clientRequest, IHttpClientFactory httpClientFactory, IClientRoleRequest clientRoleRequest)
    {
        ClientTokenRequest = clientTokenRequest;
        RealmRoleRequest = realmRoleRequest;
        UserRealmRoleRequest = userRoleRequest;
        UserRequest = userRequest;
        ClientRequest = clientRequest;
        _httpClientFactory = httpClientFactory;
        ClientRoleRequest = clientRoleRequest;
    }

    public async Task<Result> RegisterUserWithRealmRole(string url, string uri, string clientId, string clientSecret, string username, string password, string[] roles, HttpClient? httpClient = null, CancellationToken cancellationToken = default)
    {
        httpClient ??=_httpClientFactory?.CreateClient();
        ArgumentNullException.ThrowIfNull(nameof(httpClient));

        var tokenResponse = await ClientTokenRequest.GetClientTokenAsync(uri, clientId, clientSecret, httpClient!, cancellationToken);
        if (!tokenResponse.IsSuccess)
            return Result.Fail(tokenResponse.StatusCode, tokenResponse.Error);
        var registerResult = await Register(username, password, tokenResponse.Content!, url, httpClient!, cancellationToken);
        if (!registerResult.IsSuccess)
            return Result.Fail(registerResult.StatusCode, registerResult.Error);
        var userResult = await UserRequest.GetUserAsync(username, tokenResponse.Content!, url, httpClient!, cancellationToken);
        if (!userResult.IsSuccess)
            return Result.Fail(userResult.StatusCode, registerResult.Error);
        foreach (var role in roles)
        {
            var response = await RealmRoleRequest.GetRealmRoleAsync(url, tokenResponse.Content!, role, httpClient!, cancellationToken);
            if (!response.IsSuccess)
                return Result.Fail(response.StatusCode, response.Error);
            var result = await UserRealmRoleRequest.AssignRealmRolesToUserAsync(userResult.Content!, role, tokenResponse.Content!, url, httpClient!, cancellationToken);
            if (!result.IsSuccess)
                return Result.Fail(result.StatusCode, result.Error);
        }
        return Result.Success(HttpStatusCode.Created);
    }
    public async Task<Result> RegisterUserWithClientRole(string url, string uri, string authClientId, string authClientSecret, string username, string password, string[] roles,string clientId, HttpClient? httpClient = null, CancellationToken cancellationToken = default)
    {
        httpClient ??= _httpClientFactory?.CreateClient();
        ArgumentNullException.ThrowIfNull(nameof(httpClient));

        var tokenResponse = await ClientTokenRequest.GetClientTokenAsync(uri, authClientId, authClientSecret, httpClient!, cancellationToken);
        if (!tokenResponse.IsSuccess)
            return Result.Fail(tokenResponse.StatusCode, tokenResponse.Error);
        var registerResult = await Register(username, password, tokenResponse.Content!, url, httpClient!);
        if (!registerResult.IsSuccess)
            return Result.Fail(registerResult.StatusCode, registerResult.Error);
        var userResult = await UserRequest.GetUserAsync(username, tokenResponse.Content!, url, httpClient!);
        if (!userResult.IsSuccess)
            return Result.Fail(userResult.StatusCode, userResult.Error);
        var clientResult = await ClientRequest.GetClientUuidAsync(clientId, tokenResponse.Content!, url, httpClient!, cancellationToken);
        if (!clientResult.IsSuccess)
            return Result.Fail(clientResult.StatusCode, clientResult.Error);
        foreach (var role in roles)
        {
            var response = await ClientRoleRequest.GetClientRoleAsync(url, tokenResponse.Content!, clientId, role, httpClient!, cancellationToken);
            if (!response.IsSuccess)
                return Result.Fail(response.StatusCode, response.Error);
            var result = await UserRealmRoleRequest.AssignRealmRolesToUserAsync(userResult.Content!, role, tokenResponse.Content!, url, httpClient!, cancellationToken);
            if (!result.IsSuccess)
                return Result.Fail(result.StatusCode, result.Error);
        }
        return Result.Success(HttpStatusCode.Created);
    }
    public async Task<Result> RegisterUserWithoutRole(string url, string uri, string authClientId, string authClientSecret, string username, string password, HttpClient? httpClient = null, CancellationToken cancellationToken = default)
    {
        httpClient ??= _httpClientFactory?.CreateClient();
        ArgumentNullException.ThrowIfNull(nameof(httpClient));

        var tokenResponse = await ClientTokenRequest.GetClientTokenAsync(uri, authClientId, authClientSecret, httpClient!, cancellationToken);
        if (!tokenResponse.IsSuccess)
            return Result.Fail(tokenResponse.StatusCode, tokenResponse.Error);
        var registerResult = await Register(username, password, tokenResponse.Content!, url, httpClient!, cancellationToken);
        if (!registerResult.IsSuccess)
            return Result.Fail(registerResult.StatusCode, registerResult.Error);
        return Result.Success(registerResult.StatusCode);
    }

    private async Task<Result> Register(string username, string password, string accessToken, string url, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        var credentials = new Credentials(password);
        var user = new RegisterUserDto()
        {
            Email = username,
            UserName = username,
            Credentials = [credentials]
        };
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var uri = $"{url}users";
        try
        {
            var response = await httpClient.PostAsJsonAsync(uri, user, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Created)
                return Result.Success(response.StatusCode);
            return Result.Fail(response.StatusCode, "Register fail");
        }
        catch (Exception ex)
        {
            return Result.Fail($"{ex.Message} Exception for register fail");
        }
    }
}
