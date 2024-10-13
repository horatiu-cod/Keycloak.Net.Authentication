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
        //services.AddSingleton<IConfigureOptions<AuthenticationOptions>, ConfigureAuthenticateSchemeOptions>()
        return services.AddAuthentication("keycloak");
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
            {
                return false;
            }
            return ValidateAudience(jwtBearerValidationOptions);
        }, message).Configure(authConfiguration);
        builder.AddOptions();

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
                {
                    return false;
                }
                return ValidateAudience(jwtBearerValidationOptions);
            },message);
        if (options != null)
            builder.AddOptions(options);
        else builder.AddOptions();
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
        builder.Services.AddTransient<IClaimsTransformation>(sp => new JwtClaimsTransformation(options));
        builder.AddJwtBearer("keycloak", options);

        return builder.Services;
    }
    public static void AddOptions(this AuthenticationBuilder builder, Action<JwtBearerOptions> options)
    {
        builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>,ConfigureJwtBearerValidationOptions>();
        builder.Services.AddTransient<IClaimsTransformation, KeycloakClaimsTransformation>();
        builder.AddJwtBearer("keycloak", options);

    }

    private static void AddOptions(this AuthenticationBuilder builder)
    {
        builder.Services.AddTransient<IClaimsTransformation, KeycloakClaimsTransformation>();
        builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerValidationOptions>();
        builder.AddJwtBearer("keycloak");
    }
    private static bool ValidateAudience(JwtBearerValidationOptions JwtOptions)
    {
        if (!string.IsNullOrEmpty(JwtOptions.Audience))
            return true;
        if (!string.IsNullOrEmpty(JwtOptions.ValidAudience))
            return true;
        if (JwtOptions.ValidAudiences is null)
            return false;
        if (JwtOptions.ValidAudiences.Length != 0)
            return true;
        return false;
    }

}
