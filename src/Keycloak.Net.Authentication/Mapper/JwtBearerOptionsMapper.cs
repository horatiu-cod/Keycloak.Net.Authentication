using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Keycloak.Net.Authentication.Mapper;

internal static class JwtBearerOptionsMapper
{
    public static void MapFromJwtBearerValidationOptions(JwtBearerValidationOptions JwtBearerValidationOptions, JwtBearerOptions options)
    {
        options.Audience = JwtBearerValidationOptions.Audience;
        options.Authority = JwtBearerValidationOptions.Authority;
        options.RequireHttpsMetadata = JwtBearerValidationOptions.RequireHttpsMetadata;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = JwtBearerValidationOptions.ValidateIssuerSigningKey,
            ValidateIssuer = JwtBearerValidationOptions.ValidateIssuer,
            ValidateAudience = JwtBearerValidationOptions.ValidateAudience,
            //ValidAudience = JwtBearerValidationOptions.ValidAudience,
            //ValidAudiences = JwtBearerValidationOptions.ValidAudiences,
            //ValidIssuer = JwtBearerValidationOptions.ValidIssuer,
            //ValidIssuers = JwtBearerValidationOptions.ValidIssuers,
            //IssuerSigningKey = JwtBearerValidationOptions.IssuerSigningKey,
            //IssuerSigningKeys = JwtBearerValidationOptions.IssuerSigningKeys
        };
    }
}