using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Keycloak.Net.Authentication.Mapper;

internal static class JwtBearerOptionsMapper
{
    public static void MapFromJwtBearerValidationOptions(JwtBearerValidationOptions JwtBearerValidationOptions, JwtBearerOptions options)
    {
        if (JwtBearerValidationOptions.Audience is not null)
            options.Audience = JwtBearerValidationOptions.Audience;
        if(JwtBearerValidationOptions.Authority is not null)
            options.Authority = JwtBearerValidationOptions.Authority;
        if(JwtBearerValidationOptions.ValidAudience is not null)
            options.TokenValidationParameters.ValidAudience = JwtBearerValidationOptions.ValidAudience;
        if (JwtBearerValidationOptions.ValidAudiences?.Length > 0)
            options.TokenValidationParameters.ValidAudiences = JwtBearerValidationOptions.ValidAudiences;
        if(JwtBearerValidationOptions.ValidIssuer is not null)
            options.TokenValidationParameters.ValidIssuer = JwtBearerValidationOptions.ValidIssuer;
        if(JwtBearerValidationOptions.ValidIssuers?.Length > 0)
            options.TokenValidationParameters.ValidIssuers = JwtBearerValidationOptions.ValidIssuers;
        options.RequireHttpsMetadata = JwtBearerValidationOptions.RequireHttpsMetadata;
        options.TokenValidationParameters.ValidateIssuerSigningKey = JwtBearerValidationOptions.ValidateIssuerSigningKey;
        options.TokenValidationParameters.ValidateIssuer = JwtBearerValidationOptions.ValidateIssuer;
        options.TokenValidationParameters.ValidateAudience = JwtBearerValidationOptions.ValidateAudience;
    }
}