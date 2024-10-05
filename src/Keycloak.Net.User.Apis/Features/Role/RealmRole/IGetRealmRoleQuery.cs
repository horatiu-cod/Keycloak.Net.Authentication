using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.Role.RealmRole
{
    internal interface IGetRealmRoleQuery
    {
        Task<Result<RoleRepresentation?>> GetRealmRoleAsync(string url, string roleName, HttpClient httpClient, CancellationToken cancellationToken = default);
        Task<Result<RoleRepresentation[]?>> GetRealmRolesAsync(string url, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}