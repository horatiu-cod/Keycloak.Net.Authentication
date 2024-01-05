using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Keycloak.Net.Authentication;

public class JwtBearerValidationOptions : JwtBearerOptions
{
    [Required]
    public new string Authority { get; set; } = string.Empty;
    [Required]
    public string ClientId { get; set; } = string.Empty;
    public string NameClaim { get; set; } = "preferred_username";
    [JsonPropertyName("TokenValidationParameters")]
    public new TokenValidationParametersOptions TokenValidationParameters { get; set; } = new TokenValidationParametersOptions();
}

public class TokenValidationParametersOptions : TokenValidationParameters
{
    [Required]
    public new string ValidAudience { get; set; } = string.Empty;
    [Required]
    public new string ValidIssuer { get; set; } = string.Empty;
    public new bool ValidateIssuerSigningKey { get; set; } = true;
}