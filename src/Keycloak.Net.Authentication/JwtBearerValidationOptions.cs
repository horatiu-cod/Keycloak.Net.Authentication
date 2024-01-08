using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace Keycloak.Net.Authentication;

public class JwtBearerValidationOptions 
{
    [Required]
    public string? ClientId { get; set; }
    [Required]
    public string? Authority { get; set; }
    public bool RequireHttpsMetadata { get; set; } = true;
    public string? Audience { get; set; }
    public HttpMessageHandler? BackchannelHttpHandler { get; set; }
    public HttpClient Backchannel { get; set; } = default!;
    public TimeSpan BackchannelTimeout { get; set; } = TimeSpan.FromMinutes(1);
    public bool SaveToken { get; set; } = false;
    public string NameClaim { get; set; } = "preferred_username";
    public TokenValidationParametersOptions TokenValidationParameters { get; set;} = new TokenValidationParametersOptions();
}

public class TokenValidationParametersOptions 
{
    [Required]
    public string? ValidAudience { get; set; }
    public string? ValidIssuer { get; set; }
    public SecurityKey? IssuerSigningKey { get; set; }
    public IEnumerable<SecurityKey>? IssuerSigningKeys { get; set; }
    public bool RequireAudience { get; set; } = true;
    public bool RequireExpirationTime { get; set; } = true;
    public bool RequireSignedTokens { get; set; } = true;
    public bool SaveSigninToken { get; set; } = false;
    public bool ValidateActor { get; set; } = false;
    public bool ValidateAudience { get; set; } = true;
    public bool ValidateIssuer { get; set; } = true;
    public bool ValidateWithLKG { get; set; } = false;
    public bool ValidateIssuerSigningKey { get; set; } = true;
    public bool ValidateLifetime { get; set; } = true;
    public bool ValidateSignatureLast { get; set; } = false;
    public bool ValidateTokenReplay { get; set; } = false;
    public IEnumerable<string>? ValidAlgorithms { get; set; }
    public IEnumerable<string>? ValidAudiences { get; set; }
    public IEnumerable<string>? ValidIssuers { get; set; }
    public IEnumerable<string>? ValidTypes { get; set; }

}