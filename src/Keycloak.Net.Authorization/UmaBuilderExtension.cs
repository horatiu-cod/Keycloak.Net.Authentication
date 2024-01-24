using Keycloak.Net.Authorization.AudienceAccess;
using Keycloak.Net.Authorization.Configuration;
using Keycloak.Net.Authorization.PermissionAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Keycloak.Net.Authorization;

public static class UmaBuilderExtension
{
    public static IServiceCollection AddUma (this IServiceCollection services, Action<ClientConfiguration> options)
    {
        services.AddHttpClient("uma");
        services.AddTransient<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddTransient<IAudienceAccessRequest, AudienceAccessRequest>();
        services.AddTransient<IPermissionRequest, PermissionRequest>();
        //services.AddAuthorization();

        var message = $"Validation failed for {nameof(ClientConfiguration)} members";

        services.AddOptionsWithValidateOnStart<ClientConfiguration>().Validate(options =>
        {
            if (string.IsNullOrEmpty(options.ClientId))
                return false;
            return true;
        }, message).Configure(options);

        return services;
    }
    public static IServiceCollection AddUma(this IServiceCollection services, string sectionName)
    {
        services.AddHttpClient("uma");
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddScoped<IAudienceAccessRequest, AudienceAccessRequest>();
        services.AddSingleton<IPermissionRequest, PermissionRequest>();

        string message = string.Empty;

        services.AddOptionsWithValidateOnStart<ClientConfiguration>().BindConfiguration(sectionName).Validate(options =>
        {
            if (string.IsNullOrEmpty(options.ClientId))
            {
                message = $"Validation failed for {nameof(options.ClientId)} member";
                return false;
            }
            return true;
        }, message);

        return services;
    }
}
