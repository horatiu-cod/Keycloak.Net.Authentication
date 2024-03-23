using Keycloak.Net.FluentApi.Common;

namespace Keycloak.Net.FluentApi.Features.User.UserRole
{
    internal interface IUserRealmRoleRequest
    {
        Task<Result> AssignRealmRolesToUserAsync(string userId, string role, string accessToken, string url, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}