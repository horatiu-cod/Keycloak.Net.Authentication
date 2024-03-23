using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.Role.UserRole
{
    internal interface IUserRoleRequest
    {
        Task<Result> AssignClientRolesToUserAsync(string userId, string clientUuid, RoleDto[] roles, string accessToken, HttpClient httpClient, CancellationToken cancellationToken);
        Task<Result> AssignRealmRolesToUserAsync(string userId, RoleDto[] roles, string accessToken, HttpClient httpClient, CancellationToken cancellationToken = default);
        Task<Result> AssignRealmRolesToUserAsync(string userId, string role, string accessToken, string url, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}