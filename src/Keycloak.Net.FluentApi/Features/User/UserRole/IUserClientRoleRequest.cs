using Keycloak.Net.FluentApi.Common;

namespace Keycloak.Net.FluentApi.Features.User.UserRole
{
    internal interface IUserClientRoleRequest
    {
        Task<Result> AssignClientRolesToUserAsync(string userId, string clientUuid, string role, string accessToken, string url, HttpClient httpClient, CancellationToken cancellationToken);
    }
}