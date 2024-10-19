using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.Role.UserRole;

internal interface IAssignRealmRoleInternalCommand
{
    Task<Result> AssignRealmRolesAsync(string url, string userId, RoleRepresentation[] roles, HttpClient httpClient, CancellationToken cancellationToken = default);
}