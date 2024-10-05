using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.Role.UserRole
{
    internal interface IAssignUserRoleCommand
    {
        Task<Result> AssignClientRolesToUserAsync(string url, string userId, string clientUuid, RoleRepresentation[] roles, HttpClient httpClient, CancellationToken cancellationToken = default);
        Task<Result> AssignRealmRolesToUserAsync(string url, string userId, RoleRepresentation[] roles, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}