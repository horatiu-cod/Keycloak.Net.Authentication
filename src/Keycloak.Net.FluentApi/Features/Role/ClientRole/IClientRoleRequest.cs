using Keycloak.Net.FluentApi.Common;

namespace Keycloak.Net.FluentApi.Features.Role.ClientRole
{
    internal interface IClientRoleRequest
    {
        Task<Result<string?>> GetClientRoleAsync(string url, string accessToken, string clientUuid, string roleName,  HttpClient httpClient, CancellationToken cancellationToken);
    }
}