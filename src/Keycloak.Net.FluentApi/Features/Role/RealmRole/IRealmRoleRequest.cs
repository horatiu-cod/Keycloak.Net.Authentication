using Keycloak.Net.FluentApi.Common;

namespace Keycloak.Net.FluentApi.Features.Role.RealmRole;

internal interface IRealmRoleRequest
{
    Task<Result<string?>> GetRealmRoleAsync(string url, string accessToken, string roleName, HttpClient httpClient, CancellationToken cancellationToken = default);
}