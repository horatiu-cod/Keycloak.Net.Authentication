using Keycloak.Net.Authorization.Common;

namespace Keycloak.Net.Authorization.PermissionAccess
{
    internal interface IPermissionRequest
    {
        Task<Result<string>> VerifyPermissionAccessAsync(string accessToken, string resource, string scope, CancellationToken cancellationToken = default);
    }
}