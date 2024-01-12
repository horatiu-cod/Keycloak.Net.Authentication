using Keycloak.Net.Authentication.Common;
using Keycloak.Net.Authentication.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Keycloak.Net.Authentication.JWT;

internal class KeycloakClaimsTransformation : IClaimsTransformation
{
    private IOptions<JwtBearerValidationOptions> JwtOptions { get; }

    public KeycloakClaimsTransformation(IOptions<JwtBearerValidationOptions> jwtOptions)
    {
        JwtOptions = jwtOptions;
    }
    /// <summary>
    /// Map and transform the keycloak jwt realm and client "roles" claim to identity "role" claim
    /// Map and transform the keycloak jwt "preferred_username" claim to identity "name" claim
    /// </summary>
    /// <param name="principal"></param>
    /// <returns>Task of System.Security.Claims.ClaimsPrincipal</returns>
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var claimsIdentity = (ClaimsIdentity?)principal.Identity;
        if (claimsIdentity is not null && ShouldTransform(claimsIdentity))
        {
            MapRoleClaim(claimsIdentity);
            MapNameClaim(claimsIdentity);
        }
        return Task.FromResult(principal);
    }
    private bool ShouldTransform(ClaimsIdentity? claimsIdentity)
    {
        if (claimsIdentity is not null && claimsIdentity.HasClaim(c => c.Type == Constants.RoleClaimType))
        {
            return false;
        }
        if (claimsIdentity is not null && claimsIdentity.TryGetClaim(c => c.Type == Constants.IssuerClaimType, out var issClaim) && issClaim is not null && JwtOptions is not null)
        {
            if (string.IsNullOrEmpty(JwtOptions.Value.Authority))
#pragma warning disable CS8604 // Possible null reference argument.
                return (string.Equals(issClaim.Value, JwtOptions.Value.ValidIssuer, StringComparison.OrdinalIgnoreCase) || string.Equals(issClaim.Value, JwtOptions.Value.ValidIssuers.Where(c => c == issClaim.Value).FirstOrDefault(), StringComparison.OrdinalIgnoreCase));
#pragma warning restore CS8604 // Possible null reference argument.
            return string.Equals(issClaim.Value, JwtOptions.Value.Authority, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }
    /// <summary>
    /// Map and transform the keycloak jwt "roles" claim to identity "role" claim type
    /// </summary>
    /// <param name="claimsIdentity"></param>
    private void MapRoleClaim(ClaimsIdentity? claimsIdentity)
    {
        var audience = GetAudience();
        if (JwtOptions is not null && claimsIdentity is not null)
        {
            var roles = TokenRoleMapper.GetRoles(claimsIdentity, audience);
            foreach (var role in roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
        }
    }
    /// <summary>
    /// Map and transform the keycloak jwt "preferred_username" claim to identity "name" claim type
    /// </summary>
    /// <param name="claimsIdentity"></param>
    private void MapNameClaim(ClaimsIdentity? claimsIdentity)
    {
        if (JwtOptions is not null && claimsIdentity is not null)
            if (claimsIdentity.TryGetClaim(c => c.Type == JwtOptions.Value.NameClaim, out var claimToSet))
            {
                var nameClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                claimsIdentity.TryRemoveClaim(nameClaim);
                if (claimToSet is not null)
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, claimToSet.Value));
                return;
            }
        return;
    }
    private string GetAudience()
    {
        if (!string.IsNullOrEmpty(JwtOptions.Value.Audience))
            return JwtOptions.Value.Audience!;
        if (!string.IsNullOrEmpty(JwtOptions.Value.ValidAudience))
            return JwtOptions.Value.ValidAudience!;
#pragma warning disable CS8604 // Possible null reference argument.
        if (JwtOptions.Value.ValidAudiences.Any())
            return JwtOptions.Value.ValidAudiences.FirstOrDefault()!;
#pragma warning restore CS8604 // Possible null reference argument.
        return string.Empty;
    }
}
