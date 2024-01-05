using System.Security.Claims;

namespace Keycloak.Net.Authentication.Extensions;

internal static class ClaimsIdentityExtensions
{
    public static bool TryGetClaim(this ClaimsIdentity claimsIdentity, Predicate<Claim> claim, out Claim? returnClaim)
    {
        if (claimsIdentity.HasClaim(claim))
        {
            returnClaim = claimsIdentity.FindFirst(claim);
            return true;
        }
        returnClaim = null;
        return false;
    }
}
