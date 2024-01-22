using Keycloak.Net.Authorization.AudienceAccess;
using Keycloak.Net.Authorization.PermissionAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Keycloak.Net.Authorization;

public static class UmaBuilderExtension
{
    public static IServiceCollection AddUma (this IServiceCollection services)
    {
        services.AddHttpClient("uma");
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddScoped<IAudienceAccessRequest, AudienceAccessRequest>();
        services.AddSingleton<IPermissionRequest, PermissionRequest>();

        return services;
    }
}
