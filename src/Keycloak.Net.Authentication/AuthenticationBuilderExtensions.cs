using Keycloak.Net.Authentication.Configuration;
using Keycloak.Net.Authentication.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;

namespace Keycloak.Net.Authentication;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddKeyCloakAuthentication(this IServiceCollection services)
    {
        services.AddSingleton<IConfigureOptions<AuthenticationOptions>, ConfigureAuthenticateSchemeOptions>();
        return services.AddAuthentication();
    }

    public static IServiceCollection AddKeyCloakJwtBearerOptions(this AuthenticationBuilder builder, Action<JwtBearerValidationOptions> authConfiguration)
    {
        IdentityModelEventSource.ShowPII = true;

        builder.Services.AddOptionsWithValidateOnStart<JwtBearerValidationOptions>().ValidateDataAnnotations().Configure(authConfiguration);
        builder.AddJwtBearerOptions();

        return builder.Services;
    }
    public static IServiceCollection AddKeyCloakJwtBearerOptions(this AuthenticationBuilder builder, string sectionName, Action<JwtBearerOptions>? options = default)
    {
        IdentityModelEventSource.ShowPII = true;

        builder.Services.AddOptionsWithValidateOnStart<JwtBearerValidationOptions>()
            .BindConfiguration(sectionName).ValidateDataAnnotations();
#pragma warning disable CS8604 // Possible null reference argument.
        builder.AddJwtBearerOptions(options);
#pragma warning restore CS8604 // Possible null reference argument.
        return builder.Services;
    }

    private static void AddJwtBearerOptions(this AuthenticationBuilder builder, Action<JwtBearerOptions> options)
    {
        builder.Services.AddTransient<IClaimsTransformation, KeycloakClaimsTransformation>();
        builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerValidationOptions>();
        builder.AddJwtBearer(options);
    }
    private static void AddJwtBearerOptions(this AuthenticationBuilder builder)
    {
        builder.Services.AddTransient<IClaimsTransformation, KeycloakClaimsTransformation>();
        builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerValidationOptions>();
        builder.AddJwtBearer();
    }

}
