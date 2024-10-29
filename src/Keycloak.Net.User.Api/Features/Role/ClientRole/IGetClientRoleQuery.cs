using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Features.Role;

namespace Keycloak.Net.User.Api.Features.Role.ClientRole
{
    internal interface IGetClientRoleQuery
    {
        //Task<Result<RoleRepresentation?>> GetClientRoleAsync(string clientUuid, string accessToken, string roleName, HttpClient httpClient, CancellationToken cancellationToken = default);
        Task<Result<RoleRepresentation?>> GetClientRoleAsync(string url, string clientUuid, string roleName, HttpClient httpClient, CancellationToken cancellationToken = default);
        Task<Result<RoleRepresentation[]?>> GetClientRolesAsync(string url, string clientUuid, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}