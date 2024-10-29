using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Features.Role;

namespace Keycloak.Net.User.Api.Features.Role.UserRole
{
    internal interface IAssignClientRoleInternalCommand
    {
        Task<Result> AssignClientRolesAsync(string url, string userId, string clientUuid, RoleRepresentation[] roles, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}