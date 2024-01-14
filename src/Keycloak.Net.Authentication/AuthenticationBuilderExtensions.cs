using Keycloak.Net.Authentication.Configuration;
using Keycloak.Net.Authentication.JWT;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;

namespace Keycloak.Net.Authentication;

public static class AuthenticationBuilderExtensions
{
    /// <summary>
    /// Configure and register <see cref=" ConfigureAuthenticateSchemeOptions"/>. Set the schemes to JwtBearerDefaults.AuthenticationScheme.
    /// <para>
    /// Adds AddAuthentication to ServiceCollection.
    /// </para>
    /// </summary>
    /// <param name="services"></param>
    /// <returns>The <see cref="AuthenticationBuilder"/> so additional calls can be chained.</returns>
    public static AuthenticationBuilder AddKeyCloakAuthentication(this IServiceCollection services)
    {
        services.AddSingleton<IConfigureOptions<AuthenticationOptions>, ConfigureAuthenticateSchemeOptions>();
        return services.AddAuthentication();
    }
    /// <summary>
    /// Adds <see cref="JwtBearerValidationOptions"/> options service.
    /// Register Action JwtBearerValidationOptions <paramref name="authConfiguration"/> to configure options.
    /// <para/>
    /// Register the <see cref="AddJwtBearerOptions"/> 
    /// <code>
    /// builder.Services
    ///     .AddKeyCloakAuthentication()
    ///     .AddKeyCloakJwtBearerOptions(c =>
    ///      {
    ///         c.Authority = "KeycloakRealmUrl";
    ///         c.Audience = "audience";
    ///      })
    /// </code>
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="authConfiguration"></param>
    /// <returns>The <see cref="IServiceCollection"/> so additional calls can be chained. A reference to <paramref name="builder"/> after the operation has completed</returns>
    public static IServiceCollection AddKeyCloakJwtBearerOptions(this AuthenticationBuilder builder, Action<JwtBearerValidationOptions> authConfiguration)
    {
        IdentityModelEventSource.ShowPII = true;
        var message = $"Validation failed for {nameof(JwtBearerValidationOptions)} members";

        builder.Services.AddOptionsWithValidateOnStart<JwtBearerValidationOptions>().Validate(jwtBearerValidationOptions =>
        {
            if (string.IsNullOrEmpty(jwtBearerValidationOptions.Authority))
                return false;
            return ValidateAudience(jwtBearerValidationOptions);
        }, message).Configure(authConfiguration);
        builder.AddJwtBearerOptions();

        return builder.Services;
    }
    /// <summary>
    /// Adds <see cref="JwtBearerValidationOptions"/> options service.
    /// Bind <see cref="JwtBearerValidationOptions"/> to settings <paramref name="authConfiguration"/> to configure options.
    /// <para/>
    /// Register the <see cref="AddJwtBearerOptions"/> 
    /// <code>
    /// Example of implementation
    ///  builder.Services
    ///     .AddKeyCloakAuthentication()
    ///     .AddKeyCloakJwtBearerOptions("appsettings_section_name")
    /// </code>
    /// Optional: configure <see cref="JwtBearerOptions"/>
    /// <code>
    ///  builder.Services
    ///     .AddKeyCloakAuthentication()
    ///     .AddKeyCloakJwtBearerOptions("appsettings_section_name", (JwtBearerOptions)options =>
    ///       {
    ///         // if set will override the settings values
    ///         options.Audience = "new_audience";
    ///         options.TokenValidationParameters.ValidAudience = "new_valid_audience";
    ///         .....
    ///       })
    ///  
    /// Example of appsettings.json 
    /// {
    /// "KeycloakUrl": "FROM_USER_SECRET",
    /// "RealmName": "FROM_USER_SECRET",
    /// 
    ///  "appsettings_section_name": {
    ///      "Authority": "{KeycloakUrl}{RealmName}",
    ///      "Audience": "audience"
    ///   }
    /// }
    /// Or
    /// {
    /// "KeycloakUrl": "FROM_USER_SECRET",
    /// "RealmName": "FROM_USER_SECRET",
    /// 
    ///  "appsettings_section_name": {
    ///      "Authority": "{KeycloakUrl}{RealmName}",
    ///      "ValidAudience": "audience"
    ///   }
    /// }
    /// Or
    /// {
    /// "KeycloakUrl": "FROM_USER_SECRET",
    /// "RealmName": "FROM_USER_SECRET",
    /// 
    ///  "appsettings_section_name": {
    ///      "Authority": "{KeycloakUrl}{RealmName}",
    ///      "ValidAudiences": ["audience"]
    ///   }
    /// }
    /// </code>
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sectionName"></param>
    /// <param name="options"></param>
    /// <returns>The <see cref="IServiceCollection"/> so additional calls can be chained. A reference to <paramref name="builder"/> after the operation has completed</returns>
    public static IServiceCollection AddKeyCloakJwtBearerOptions(this AuthenticationBuilder builder, string sectionName, Action<JwtBearerOptions>? options = default)
    {
        IdentityModelEventSource.ShowPII = true;
        var message = $"Validation failed for {sectionName} members";
        builder.Services.AddOptionsWithValidateOnStart<JwtBearerValidationOptions>()
            .BindConfiguration(sectionName).Validate(jwtBearerValidationOptions =>
            {
                if (string.IsNullOrEmpty(jwtBearerValidationOptions.Authority))
                    return false;
                return ValidateAudience(jwtBearerValidationOptions);
            },message);
#pragma warning disable CS8604 // Possible null reference argument.
        builder.AddJwtBearerOptions(options);
#pragma warning restore CS8604 // Possible null reference argument.
        return builder.Services;
    }
    /// <summary>
    /// Register service <see cref="AddJwtBearerOptions"/> 
    /// <code>
    /// Example of implementation
    ///  builder.Services
    ///     .AddKeyCloakAuthentication()
    ///     .AddKeyCloakJwtBearerOptions((JwtBearerOptions)options =>
    ///       {
    ///         options.Authority = "KeycloakRealmUrl";
    ///         .....
    ///         options.TokenValidationParameters = new TokenValidationParameters
    ///         { 
    ///         ValidAudience = "valid_audience";
    ///         .....
    ///       })
    /// </code>
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <returns>The <see cref="IServiceCollection"/> so additional calls can be chained.</returns>
    private static IServiceCollection AddKeyCloakJwtBearerOptions(this AuthenticationBuilder builder, Action<JwtBearerOptions> options)
    {
        IdentityModelEventSource.ShowPII = true;

        builder.AddJwtBearerOptions(options);
        return builder.Services;
    }
    /// <summary>
    /// Register <see cref="KeycloakClaimsTransformation"/> and <see cref="ConfigureJwtBearerValidationOptions"/>.
    /// <para></para>
    /// Pass the <paramref name="options"/> to AddJwtBearer(<paramref name="options"/>)
    /// <code>
    /// Example of implementation
    ///  builder.Services
    ///     .AddKeyCloakAuthentication()
    ///     .AddJwtBearerOptions((JwtBearerOptions)options =>
    ///       {
    ///         options.Authority = "KeycloakRealmUrl";
    ///         .....
    ///         options.TokenValidationParameters = new TokenValidationParameters
    ///         { 
    ///         ValidAudience = "valid_audience";
    ///         .....
    ///       })
    /// </code>
    /// </summary>
    /// <param name="builder">AuthenticationBuilder</param>
    /// <param name="options"></param>
    /// <returns>The <see cref="IServiceCollection"/> so additional calls can be chained.</returns>
    public static IServiceCollection AddJwtBearerOptions(this AuthenticationBuilder builder, Action<JwtBearerOptions> options)
    {
        builder.Services.AddTransient<IClaimsTransformation, KeycloakClaimsTransformation>();
        builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerValidationOptions>();
        builder.AddJwtBearer(options);

        return builder.Services;
    }
    private static void AddJwtBearerOptions(this AuthenticationBuilder builder)
    {
        builder.Services.AddTransient<IClaimsTransformation, KeycloakClaimsTransformation>();
        builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerValidationOptions>();
        builder.AddJwtBearer();
    }
    private static bool ValidateAudience(JwtBearerValidationOptions JwtOptions)
    {
        if (!string.IsNullOrEmpty(JwtOptions.Audience))
            return true;
        if (!string.IsNullOrEmpty(JwtOptions.ValidAudience))
            return true;
        if (JwtOptions.ValidAudiences is null)
            return false;
        if (JwtOptions.ValidAudiences.Any())
            return true;
        return false;
    }

}
