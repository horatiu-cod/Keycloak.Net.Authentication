using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Features.Role;

namespace Keycloak.Net.User.Api.Features.Role.UserRole;

internal interface IAssignRealmRoleInternalCommand
{
    Task<Result> AssignRealmRolesAsync(string url, string userId, RoleRepresentation[] roles, HttpClient httpClient, CancellationToken cancellationToken = default);
}