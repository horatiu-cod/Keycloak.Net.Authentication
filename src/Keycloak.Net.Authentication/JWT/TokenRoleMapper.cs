using System.Security.Claims;
using System.Text.Json;
using Keycloak.Net.Authentication.Common;
using Keycloak.Net.Authentication.Extensions;

namespace Keycloak.Net.Authentication.JWT;

internal static class TokenRoleMapper
{
    private static readonly Predicate<Claim> ClientResourceAccessAdapter = claim => claim.Type == Constants.ResourceAccessClaimType;
    private static readonly Predicate<Claim> RealmResourceAdapter = claim => claim.Type == Constants.RealmAccessClaimType;

    private static readonly string RoleKeyName = Constants.RoleKeyName;

    /// <summary>
    /// Get all KeyCloak Roles Claims from JWT
    /// </summary>
    /// <param name="identity"></param>
    /// <param name="audience"></param>
    /// <returns >A string[] of Roles</returns>
    public static string[] GetRoles(ClaimsIdentity identity, string? audience)
    {
        var roles = new List<string>();
        if (audience is not null)
        { 
            var clientRoles = GetClientRoles(identity, audience);
            if (clientRoles.Any())
            {
                foreach (var role in clientRoles) 
                {
                    roles.Add(role);
                }
            }
        }
        var realmRoles = GetRealmRoles(identity);
        if (realmRoles.Any())
        {
            foreach(var role in realmRoles)
            {
                roles.Add(role);
            }
        }
        return roles.ToArray();
    }
    /// <summary>
    /// Map and retrieve client roles in the resource_access claim
    /// <code>
    ///Example of keycloack resource_access claim
    ///  "resource_access": {
    ///     "client_aud": {
    ///         "roles": [
    ///             "role1", 
    ///             "role2" 
    ///         ]
    ///     },
    ///     "account": {
    ///         "roles": [
    ///             "view-profile"
    ///         ]
    ///     }
    /// }
    /// </code>
    /// </summary>
    /// <param name="identity"></param>
    /// <param name="audience"></param>
    /// <returns>A string[] of client roles</returns>
    private static List<string> GetClientRoles(ClaimsIdentity identity, string audience)
    {
        var returnRoles = new List<string>();
        if (audience is null)
            return returnRoles;
        if (identity.TryGetClaim(ClientResourceAccessAdapter, out var resourceAccessClaim))
        {
            if (resourceAccessClaim is not null)
            {
                var resource = JsonDocument.Parse(resourceAccessClaim.Value);
                var audienceResourceExist = resource
                    .RootElement
                    .TryGetProperty(audience, out var rolesElement);
                if (audienceResourceExist)
                {
                    var roles = rolesElement.GetProperty(RoleKeyName).EnumerateArray();
                    foreach ( var role in roles )
                    {
                        var value = role.GetString();
                        if(value is not null)
                            returnRoles.Add(value);
                    }
                }
            }
        }
        return returnRoles;
    }
    /// <summary>
    /// Map and retrieve realm roles in the realm_access claim
    /// <code>
    /// Example of keycloack resource_access claim
    /// "realm_access": {
    ///         "roles": [
    ///             "role1",
    ///             "role2" 
    ///         ]
    ///     }
    /// }
    /// </code>
    /// </summary>
    /// <param name="identity"></param>
    /// <param name="audience"></param>
    /// <returns>A string[] of roles</returns>
    private static List<string> GetRealmRoles(ClaimsIdentity identity)
    {
        var returnRoles = new List<string>();

        if (identity.TryGetClaim(RealmResourceAdapter, out var resourceAccessClaim))
        {
            if (resourceAccessClaim is not null)
            {
                var resource = JsonDocument.Parse(resourceAccessClaim.Value);
                var rolesElementExist = resource
                    .RootElement
                    .TryGetProperty(RoleKeyName, out var roles);
                if (rolesElementExist)
                {
                    foreach (var role in roles.EnumerateArray())
                    {
                        var value = role.GetString();
                        if (value is not null)
                            returnRoles.Add(value);
                    }
                }
            }
        }
        return returnRoles;
    }
}
