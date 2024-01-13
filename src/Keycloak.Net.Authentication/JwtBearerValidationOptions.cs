using Keycloak.Net.Authentication.Common;
using Microsoft.IdentityModel.Tokens;

namespace Keycloak.Net.Authentication;

public record JwtBearerValidationOptions()
{
    #region JwtBearerOptions
    public string? Authority { get; set; }
    public string? Audience { get; set; }
    public string NameClaim { get; set; } = Constants.NameClaimType;
    #endregion

    #region TokenValidationParameters

    public string? ValidAudience { get; set; }
    public string? ValidIssuer { get; set; }
    public string[]? ValidAudiences { get; set; }
    public string[]? ValidIssuers { get; set; }
    public SecurityKey? IssuerSigningKey { get; set; }
    public SecurityKey[]? IssuerSigningKeys { get; set; }
    public bool ValidateIssuerSigningKey { get; set; } = true;
    #endregion
}