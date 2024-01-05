using System.Security.Claims;
using System.Text.Json;
using Keycloak.Net.Authentication.Extensions;

namespace Keycloak.Net.Authentication.Utils;

internal static class TokenRoleMapper
{
    private static readonly Predicate<Claim> ResourceAccessAdapter = claim => claim.Type == "resource_access";
    private static readonly string RoleKeyName = "roles";

    public static string[] GetRoles(ClaimsIdentity identity, string clientId)
    {
        var returnRoles = Array.Empty<string>();

        if (identity.TryGetClaim(ResourceAccessAdapter, out var resourseAccessClaim))
        {
            if (resourseAccessClaim is not null)
            {
                var resourceAccess = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string[]>>>(resourseAccessClaim.Value);
                if (resourceAccess is not null && resourceAccess.TryGetValue(clientId, out var clientResourceAccess) && clientResourceAccess.TryGetValue(RoleKeyName, out var roles))
                {
                    returnRoles = roles;
                }
            }
        }
        return returnRoles;
    }
}
