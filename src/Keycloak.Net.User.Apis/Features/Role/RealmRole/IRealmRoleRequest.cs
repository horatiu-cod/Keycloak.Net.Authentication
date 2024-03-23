using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.Role.RealmRole
{
    internal interface IRealmRoleRequest
    {
        Task<Result<RoleDto>> GetRealmRoleAsync(string accessToken, string roleName, HttpClient httpClient, CancellationToken cancellationToken = default);
        Task<Result<string?>> GetRealmRoleAsync(string url, string accessToken, string roleName, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}