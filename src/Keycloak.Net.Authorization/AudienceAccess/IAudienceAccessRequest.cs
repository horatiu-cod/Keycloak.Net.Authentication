using Keycloak.Net.Authorization.Common;

namespace Keycloak.Net.Authorization.AudienceAccess;

public interface IAudienceAccessRequest
{
    Task<Result<string>> VerifyRealmAccess(string audience, string accessToken, CancellationToken cancellationToken = default);
}
