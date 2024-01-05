using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace Keycloak.Net.Authentication;

internal static class Mapper
{
    public static JwtBearerOptions MapFromJwtBearerValidationOptions(this JwtBearerValidationOptions values)
    {
        return new JwtBearerOptions
        {
            Audience = values.Audience,
            Authority = values.Authority,
            AutomaticRefreshInterval = values.AutomaticRefreshInterval,
            ForwardAuthenticate = values.ForwardAuthenticate,
            MetadataAddress = values.MetadataAddress,
            Backchannel = values.Backchannel,
            BackchannelHttpHandler = values.BackchannelHttpHandler,
            BackchannelTimeout = values.BackchannelTimeout,
            Challenge = values.Challenge,
            ClaimsIssuer = values.ClaimsIssuer,
            Configuration = values.Configuration,
            ConfigurationManager = values.ConfigurationManager,
            Events = values.Events,
            EventsType = values.EventsType,
            ForwardChallenge = values.ForwardChallenge,
            ForwardDefault = values.ForwardDefault,
            ForwardDefaultSelector = values.ForwardDefaultSelector,
            ForwardForbid = values.ForwardForbid,
            ForwardSignIn = values.ForwardSignIn,
            ForwardSignOut = values.ForwardSignOut,
            IncludeErrorDetails = values.IncludeErrorDetails,
            MapInboundClaims = values.MapInboundClaims,
            RefreshInterval = values.RefreshInterval,
            RefreshOnIssuerKeyNotFound = values.RefreshOnIssuerKeyNotFound,
            RequireHttpsMetadata = values.RequireHttpsMetadata,
            SaveToken = values.SaveToken,
            TimeProvider = values.TimeProvider,
            UseSecurityTokenValidators = values.UseSecurityTokenValidators,
        };
    }
    public static TokenValidationParameters MapFromTokenValidationParametersOptions(this JwtBearerValidationOptions values)
    {
        return new TokenValidationParameters
        {
            AlgorithmValidator = values.TokenValidationParameters.AlgorithmValidator,
            ActorValidationParameters = values.TokenValidationParameters.ActorValidationParameters?.Clone(),
            AudienceValidator = values.TokenValidationParameters.AudienceValidator,
            ClockSkew = values.TokenValidationParameters.ClockSkew,
            ConfigurationManager = values.TokenValidationParameters.ConfigurationManager,
            CryptoProviderFactory = values.TokenValidationParameters.CryptoProviderFactory,
            DebugId = values.TokenValidationParameters.DebugId,
            IncludeTokenOnFailedValidation = values.TokenValidationParameters.IncludeTokenOnFailedValidation,
            IgnoreTrailingSlashWhenValidatingAudience = values.TokenValidationParameters.IgnoreTrailingSlashWhenValidatingAudience,
            IssuerSigningKey = values.TokenValidationParameters.IssuerSigningKey,
            IssuerSigningKeyResolver = values.TokenValidationParameters.IssuerSigningKeyResolver,
            IssuerSigningKeyResolverUsingConfiguration = values.TokenValidationParameters.IssuerSigningKeyResolverUsingConfiguration,
            IssuerSigningKeys = values.TokenValidationParameters.IssuerSigningKeys,
            IssuerSigningKeyValidator = values.TokenValidationParameters.IssuerSigningKeyValidator,
            IssuerSigningKeyValidatorUsingConfiguration = values.TokenValidationParameters.IssuerSigningKeyValidatorUsingConfiguration,
            IssuerValidator = values.TokenValidationParameters.IssuerValidator,
            IssuerValidatorUsingConfiguration = values.TokenValidationParameters.IssuerValidatorUsingConfiguration,
            LifetimeValidator = values.TokenValidationParameters.LifetimeValidator,
            LogTokenId = values.TokenValidationParameters.LogTokenId,
            LogValidationExceptions = values.TokenValidationParameters.LogValidationExceptions,
            NameClaimType = values.TokenValidationParameters.NameClaimType,
            NameClaimTypeRetriever = values.TokenValidationParameters.NameClaimTypeRetriever,
            PropertyBag = values.TokenValidationParameters.PropertyBag,
            RefreshBeforeValidation = values.TokenValidationParameters.RefreshBeforeValidation,
            RequireAudience = values.TokenValidationParameters.RequireAudience,
            RequireExpirationTime = values.TokenValidationParameters.RequireExpirationTime,
            RequireSignedTokens = values.TokenValidationParameters.RequireSignedTokens,
            RoleClaimType = values.TokenValidationParameters.RoleClaimType,
            RoleClaimTypeRetriever = values.TokenValidationParameters.RoleClaimTypeRetriever,
            SaveSigninToken = values.TokenValidationParameters.SaveSigninToken,
            SignatureValidator = values.TokenValidationParameters.SignatureValidator,
            SignatureValidatorUsingConfiguration = values.TokenValidationParameters.SignatureValidatorUsingConfiguration,
            TokenDecryptionKey = values.TokenValidationParameters.TokenDecryptionKey,
            TokenDecryptionKeyResolver = values.TokenValidationParameters.TokenDecryptionKeyResolver,
            TokenDecryptionKeys = values.TokenValidationParameters.TokenDecryptionKeys,
            TokenReader = values.TokenValidationParameters.TokenReader,
            TokenReplayCache = values.TokenValidationParameters.TokenReplayCache,
            TokenReplayValidator = values.TokenValidationParameters.TokenReplayValidator,
            TransformBeforeSignatureValidation = values.TokenValidationParameters.TransformBeforeSignatureValidation,
            TryAllIssuerSigningKeys = values.TokenValidationParameters.TryAllIssuerSigningKeys,
            TypeValidator = values.TokenValidationParameters.TypeValidator,
            ValidateActor = values.TokenValidationParameters.ValidateActor,
            ValidateAudience = values.TokenValidationParameters.ValidateAudience,
            ValidateIssuer = values.TokenValidationParameters.ValidateIssuer,
            ValidateIssuerSigningKey = values.TokenValidationParameters.ValidateIssuerSigningKey,
            ValidateLifetime = values.TokenValidationParameters.ValidateLifetime,
            ValidateSignatureLast = values.TokenValidationParameters.ValidateSignatureLast,
            ValidateTokenReplay = values.TokenValidationParameters.ValidateTokenReplay,
            ValidateWithLKG = values.TokenValidationParameters.ValidateWithLKG,
            ValidAlgorithms = values.TokenValidationParameters.ValidAlgorithms,
            ValidAudience = values.TokenValidationParameters.ValidAudience,
            ValidAudiences = values.TokenValidationParameters.ValidAudiences,
            ValidIssuer = values.TokenValidationParameters.ValidIssuer,
            ValidIssuers = values.TokenValidationParameters.ValidIssuers,
            ValidTypes = values.TokenValidationParameters.ValidTypes,
        };
}
}