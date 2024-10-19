using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Role;

namespace Keycloak.Net.User.Apis.Features.Role.UserRole
{
    internal interface IAssignClientRoleInternalCommand
    {
        Task<Result> AssignClientRolesAsync(string url, string userId, string clientUuid, RoleRepresentation[] roles, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}