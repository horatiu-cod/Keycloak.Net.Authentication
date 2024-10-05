using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.Role.ClientRole
{
    internal interface IGetClientRoleQuery
    {
        //Task<Result<RoleRepresentation?>> GetClientRoleAsync(string clientUuid, string accessToken, string roleName, HttpClient httpClient, CancellationToken cancellationToken = default);
        Task<Result<RoleRepresentation?>> GetClientRoleAsync(string url, string clientUuid, string roleName, HttpClient httpClient, CancellationToken cancellationToken = default);
        Task<Result<RoleRepresentation[]?>> GetClientRolesAsync(string url, string clientUuid, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}