using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Keycloak.Net.Authentication.Mapper;

internal static class JwtBearerOptionsMapper
{
    public static void MapFromJwtBearerValidationOptions(JwtBearerValidationOptions JwtBearerValidationOptions, JwtBearerOptions options)
    {

        options.Audience = JwtBearerValidationOptions.Audience;
        options.Authority = JwtBearerValidationOptions.Authority;
        options.Backchannel = JwtBearerValidationOptions.Backchannel;
        options.BackchannelHttpHandler = JwtBearerValidationOptions.BackchannelHttpHandler;
        options.BackchannelTimeout = JwtBearerValidationOptions.BackchannelTimeout;
        options.RequireHttpsMetadata = JwtBearerValidationOptions.RequireHttpsMetadata;
        options.SaveToken = JwtBearerValidationOptions.SaveToken;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            RequireAudience = JwtBearerValidationOptions.TokenValidationParameters.RequireAudience,
            RequireExpirationTime = JwtBearerValidationOptions.TokenValidationParameters.RequireExpirationTime,
            RequireSignedTokens = JwtBearerValidationOptions.TokenValidationParameters.RequireSignedTokens,
            SaveSigninToken = JwtBearerValidationOptions.TokenValidationParameters.SaveSigninToken,
            ValidateActor = JwtBearerValidationOptions.TokenValidationParameters.ValidateActor,
            ValidateAudience = JwtBearerValidationOptions.TokenValidationParameters.ValidateAudience,
            ValidateIssuer = JwtBearerValidationOptions.TokenValidationParameters.ValidateIssuer,
            ValidateIssuerSigningKey = JwtBearerValidationOptions.TokenValidationParameters.ValidateIssuerSigningKey,
            ValidateLifetime = JwtBearerValidationOptions.TokenValidationParameters.ValidateLifetime,
            ValidateSignatureLast = JwtBearerValidationOptions.TokenValidationParameters.ValidateSignatureLast,
            ValidateTokenReplay = JwtBearerValidationOptions.TokenValidationParameters.ValidateTokenReplay,
            ValidateWithLKG = JwtBearerValidationOptions.TokenValidationParameters.ValidateWithLKG,
            ValidAlgorithms = JwtBearerValidationOptions.TokenValidationParameters.ValidAlgorithms,
            ValidAudience = JwtBearerValidationOptions.TokenValidationParameters.ValidAudience,
            ValidAudiences = JwtBearerValidationOptions.TokenValidationParameters.ValidAudiences,
            ValidIssuer = JwtBearerValidationOptions.TokenValidationParameters.ValidIssuer,
            ValidIssuers = JwtBearerValidationOptions.TokenValidationParameters.ValidIssuers,
            ValidTypes = JwtBearerValidationOptions.TokenValidationParameters.ValidTypes,

            
        };
    }
}