using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Configuration;
using Keycloak.Net.User.Api.Features.Client.ClientAccessToken;
using Keycloak.Net.User.Api.Features.Role;
using Keycloak.Net.User.Api.Features.Role.RealmRole;
using Keycloak.Net.User.Api.Features.Role.UserRole;
using Keycloak.Net.User.Api.Features.User.Get;

namespace Keycloak.Net.User.Api.Features.User.AssignRole;

internal class AssignRealmRoleCommand(IHttpClientFactory httpClientFactory, IGetUserIdQuery getUserQuery, IAssignRealmRoleInternalCommand assignClientRoleInternalCommand, IOptionsMonitor<Server> server, IGetClientTokenQuery getClientTokenQuery, IOptionsMonitor<AdminClient> adminClient, IGetRealmRoleQuery getRealmRoleQuery) : IAssignRealmRoleCommand
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IGetUserIdQuery _getUserQuery = getUserQuery;
    private readonly IAssignRealmRoleInternalCommand _assignClientRoleInternalCommand = assignClientRoleInternalCommand;
    private readonly IGetClientTokenQuery _getClientTokenQuery = getClientTokenQuery;
    private readonly IGetRealmRoleQuery _getRealmRoleQuery = getRealmRoleQuery;
    private readonly Server _server = server.CurrentValue;
    private readonly AdminClient _adminClient = adminClient.CurrentValue;

    public async Task<Result> Handler(string username, string[] roleNames, CancellationToken cancellationToken = default)
    {
        var authUrl = BaseUrl.TokenUrl(_server.BaseAddress, _server.RealmName);
        var adminUrl = BaseUrl.AdminUrl(_server.BaseAddress, _server.RealmName);
        var httpClient = _httpClientFactory.CreateClient("kapi");

        var client = new GetClientTokenRequest(_adminClient.ClientId, _adminClient.ClientSecret);
        var tokenResponse = await _getClientTokenQuery.GetClientTokenAsync(authUrl, client, httpClient);
        if (!tokenResponse.IsSuccess)
            return Result.Fail(tokenResponse.StatusCode, tokenResponse.Error);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Content!.AccessToken!);

        var roles = new List<RoleRepresentation>();
        var roleResponse = await _getRealmRoleQuery.GetRealmRolesAsync(adminUrl, httpClient);
        if (!roleResponse.IsSuccess)
            return Result.Fail(roleResponse.StatusCode, roleResponse.Error);
        if (roleResponse.IsSuccess && roleResponse.Content is not null && roleResponse.Content.Length > 0)
        {
            foreach (var roleName in roleNames)
            {
                var role = roleResponse.Content.SingleOrDefault(r => r.Name!.Equals(roleName));
                if (role != null)
                    roles.Add(role);
            }
            if (roles.Count == 0)
                return Result.Fail(HttpStatusCode.NotFound, "Role not found");
        }

        var userIdResponse = await _getUserQuery.GetUserIdAsync(adminUrl, username, httpClient, cancellationToken);
        if (!userIdResponse.IsSuccess)
            return Result.Fail(userIdResponse.StatusCode, userIdResponse.Error);
        var response = await _assignClientRoleInternalCommand.AssignRealmRolesAsync(adminUrl, userIdResponse.Content!, [.. roles], httpClient, cancellationToken);
        if (!response.IsSuccess)
            return Result.Fail(response.StatusCode, response.Error);
        return Result.Success(response.StatusCode);
    }
}
