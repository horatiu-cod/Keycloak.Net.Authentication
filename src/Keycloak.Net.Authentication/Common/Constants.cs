namespace Keycloak.Net.Authentication.Common;

internal static class Constants
{
    /// <summary>
    /// JWT Client Resource Access Claim
    /// </summary>
    public const string ResourceAccessClaimType = "resource_access";
    /// <summary>
    /// JWT Realm Resource Access Claim
    /// </summary>
    public const string RealmAccessClaimType = "realm_access";
    /// <summary>
    /// KeyCloak JWT Roles Claim
    /// </summary>
    public const string RoleKeyName = "roles";
    /// <summary>
    /// Identity JWT Role Claim
    /// </summary>
    public const string RoleClaimType = "role";
    /// <summary>
    /// JWT Issuer Claim
    /// </summary>
    public const string IssuerClaimType = "iss";
    /// <summary>
    /// JWT Name Claim
    /// </summary>
    public const string NameClaimType = "preferred_username";
}
