using Keycloak.Net.Authentication.Configuration;
using Keycloak.Net.Authentication.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;

namespace Keycloak.Net.Authentication;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddKeycloakAuthentication(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<AuthenticationOptions>, ConfigureAuthenticateSchemeOptions>();
        return services.AddAuthentication();
    }

    public static IServiceCollection AddKeycloakJwtBearerOptions(this AuthenticationBuilder builder, Action<JwtBearerValidationOptions> authConfiguration)
    {
        IdentityModelEventSource.ShowPII = true;
        builder.Services.AddOptionsWithValidateOnStart<JwtBearerValidationOptions>().ValidateDataAnnotations().Configure(authConfiguration);
        builder.AddJwtBearerOptions();

        return builder.Services;
    }
    public static IServiceCollection AddKeycloakJwtBearerOptions(this AuthenticationBuilder builder, string? sectionName = null)
    {
        IdentityModelEventSource.ShowPII = true;

        var section = sectionName is null ? "JwtBearerOptions" : sectionName;
        builder.Services.AddOptionsWithValidateOnStart<JwtBearerValidationOptions>()
            .BindConfiguration(section).ValidateDataAnnotations();
        builder.AddJwtBearerOptions();
        return builder.Services;
    }
    public static IServiceCollection AddKeycloakJwtBearerOptions(this AuthenticationBuilder builder, IConfiguration configuration, string? sectionName = null)
    {
        IdentityModelEventSource.ShowPII = true;

        var section = sectionName is null ? "JwtBearerOptions" : sectionName;
        builder.Services.AddOptionsWithValidateOnStart<JwtBearerValidationOptions>()
            .Bind(configuration.GetSection(section)).ValidateDataAnnotations();
        builder.AddJwtBearerOptions();
        return builder.Services;
    }


    private static void AddJwtBearerOptions(this AuthenticationBuilder builder)
    {
        builder.Services.AddScoped<IClaimsTransformation, KeycloakClaimsTransformation>();
        builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerValidationOptions>();
        builder.AddJwtBearer();
    }
}
