using Keycloak.Net.Authorization.AudienceAccess;
using Keycloak.Net.Authorization.Configuration;
using Keycloak.Net.Authorization.PermissionAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Keycloak.Net.Authorization;

public static class UmaBuilderExtension
{
    public static IServiceCollection AddUma (this IServiceCollection services, Action<ClientConfiguration> options, Action<AuthorizationOptions>? configure = default)
    {
        services.AddHttpClient("uma");
        services.AddScoped<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddScoped<IAudienceAccessRequest, AudienceAccessRequest>();
        services.AddScoped<IPermissionRequest, PermissionRequest>();

        var message = $"Validation failed for {nameof(ClientConfiguration.ClientId)}";

        services.AddOptionsWithValidateOnStart<ClientConfiguration>().Validate(options =>
        {
            if (string.IsNullOrEmpty(options.ClientId))
            {
                return false;
            }
            return true;
        }, message).Configure(options);
        if (configure != null)
        {
            return services.AddAuthorization(configure);

        }
        return services.AddAuthorization();
    }
    public static IServiceCollection AddUma(this IServiceCollection services, string sectionName, Action<AuthorizationOptions>? configure = default)
    {
        services.AddHttpClient("uma");
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddScoped<IAudienceAccessRequest, AudienceAccessRequest>();
        services.AddScoped<IPermissionRequest, PermissionRequest>();

        string message = $"Validation failed for {nameof(ClientConfiguration.ClientId)}";

        services.AddOptionsWithValidateOnStart<ClientConfiguration>().BindConfiguration(sectionName).Validate(options =>
        {
            if (string.IsNullOrEmpty(options.ClientId))
            {
                return false;
            }
            return true;
        }, message);
        if (configure != null)
        {
            return services.AddAuthorization(configure);

        }
        return services.AddAuthorization();
    }
    public static IServiceCollection AddUma(this IServiceCollection services, Action<AuthorizationOptions>? configure = default)
    {
        services.AddHttpClient("uma");
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddScoped<IAudienceAccessRequest, AudienceAccessRequest>();
        services.AddScoped<IPermissionRequest, PermissionRequest>();

        if (configure != null)
        {
            return services.AddAuthorization(configure);

        }
        return services.AddAuthorization();
    }
}
