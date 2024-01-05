using Keycloak.Net.Authentication.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Keycloak.Net.Authentication.Utils;

internal class KeycloakClaimsTransformation : IClaimsTransformation
{
    private IOptions<JwtBearerValidationOptions>? JwtOptions { get; }

    public KeycloakClaimsTransformation(IOptions<JwtBearerValidationOptions>? jwtOptions)
    {
        JwtOptions = jwtOptions;
    }

    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var claimsIdentity = (ClaimsIdentity?)principal.Identity;
        if (claimsIdentity == null && ShouldTransform(claimsIdentity))
        {
            //TransformAndAddClaim(claimsIdentity);
            MapRoleClaim(claimsIdentity);
            //MapNameClaim(claimsIdentity);
        }
        return Task.FromResult(principal);
    }
    private bool ShouldTransform(ClaimsIdentity? claimsIdentity)
    {
        if (claimsIdentity is not null && claimsIdentity.HasClaim(c => c.Type == "role"))
        {
            return false;
        }
        if (claimsIdentity is not null && claimsIdentity.TryGetClaim(c => c.Type == "iss", out var issClaim) && issClaim is not null && JwtOptions is not null)
        {
            return string.Equals(issClaim.Value, JwtOptions.Value.TokenValidationParameters.ValidIssuer, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }
    private void TransformAndAddClaim(ClaimsIdentity? claimsIdentity)
    {
        throw new NotImplementedException();
        //claimsIdentity.AddClaim(new Claim(nameof(KeycloakClaimsTransformation), "true"));
    }
    private void MapRoleClaim(ClaimsIdentity? claimsIdentity)
    {
        if (JwtOptions is not null && JwtOptions.Value.ClientId is not null && claimsIdentity is not null)
        {
            var roles = TokenRoleMapper.GetRoles(claimsIdentity, JwtOptions.Value.ClientId);
            foreach (var role in roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
        }
    }
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
}
