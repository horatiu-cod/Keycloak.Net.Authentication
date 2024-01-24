using Keycloak.Net.Authorization.Common;

namespace Keycloak.Net.Authorization.AudienceAccess;

internal interface IAudienceAccessRequest
{
    Task<Result<string>> VerifyRealmAccess(string audience, string accessToken, CancellationToken cancellationToken = default);
}
