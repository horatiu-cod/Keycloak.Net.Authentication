using Microsoft.Extensions.Logging;

namespace Keycloak.Net.User.Api.Features.User.AssignRole;

internal class AssignClientRoleCommand(IHttpClientFactory httpClientFactory, IGetClientIdQuery getClientIdQuery, IGetUserIdQuery getUserQuery, IAssignClientRoleInternalCommand assignClientRoleInternalCommand, IOptionsMonitor<Server> server, IGetClientTokenQuery getClientTokenQuery, IOptionsMonitor<AdminClient> adminClient, IGetClientRoleQuery getClientRoleQuery, ILoggerFactory loggerFactory) : IAssignClientRoleCommand
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IGetClientIdQuery _getClientIdQuery = getClientIdQuery;
    private readonly IGetUserIdQuery _getUserIdQuery = getUserQuery;
    private readonly IAssignClientRoleInternalCommand _assignClientRoleInternalCommand = assignClientRoleInternalCommand;
    private readonly IGetClientTokenQuery _getClientTokenQuery = getClientTokenQuery;
    private readonly IGetClientRoleQuery _getClientRoleQuery = getClientRoleQuery;
    private readonly ILogger _logger = loggerFactory.CreateLogger<AssignClientRoleCommand>();
    private readonly Server _server = server.CurrentValue;
    private readonly AdminClient _adminClient = adminClient.CurrentValue;

    public async Task<Result> Handler(string username, string clientId, string[] roleNames, CancellationToken cancellationToken = default)
    {
        var authUrl = BaseUrl.TokenUrl(_server.BaseAddress, _server.RealmName);
        var adminUrl = BaseUrl.AdminUrl(_server.BaseAddress, _server.RealmName);
        var httpClient = _httpClientFactory.CreateClient("kapi");

        var client = new GetClientTokenRequest(_adminClient.ClientId, _adminClient.ClientSecret);
        var tokenResponse = await _getClientTokenQuery.GetClientTokenAsync(authUrl, client, httpClient);
        _logger.LogInformation("Client retrieved - {Token}", tokenResponse.IsSuccess);
        if (!tokenResponse.IsSuccess)
            return Result.Fail(tokenResponse.StatusCode, tokenResponse.Error);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Content!.AccessToken!);


        var clientIdResponse = await _getClientIdQuery.GetClientIdAsync(adminUrl, clientId, httpClient, cancellationToken);
        if (!clientIdResponse.IsSuccess)
            return Result.Fail(clientIdResponse.StatusCode, clientIdResponse.Error);

        var roles = new List<RoleRepresentation>();
        foreach (var role in roleNames)
        {
            var roleResponse = await _getClientRoleQuery.GetClientRoleAsync(adminUrl, clientIdResponse.Content!.Id, role, httpClient);
            if (!roleResponse.IsSuccess)
                return Result.Fail(roleResponse.StatusCode, roleResponse.Error);
            if (roleResponse.IsSuccess && roleResponse.Content != null)
                roles.Add(roleResponse.Content);
        }

        var userIdResponse = await _getUserIdQuery.GetUserIdAsync(adminUrl, username, httpClient, cancellationToken);
        if (!userIdResponse.IsSuccess)
            return Result.Fail(userIdResponse.StatusCode, userIdResponse.Error);
        var response = await _assignClientRoleInternalCommand.AssignClientRolesAsync(adminUrl, userIdResponse.Content!, clientIdResponse.Content!.Id, [.. roles], httpClient, cancellationToken);
        if (!response.IsSuccess)
            return Result.Fail(response.StatusCode, response.Error);
        return Result.Success(response.StatusCode);
    }
}