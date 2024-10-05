using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;
using Keycloak.Net.User.Apis.Features.Client.ClientRequest;
using Keycloak.Net.User.Apis.Features.Role.ClientRole;
using Keycloak.Net.User.Apis.Features.Role.RealmRole;
using Keycloak.Net.User.Apis.Features.Role.UserRole;
using Microsoft.Extensions.DependencyInjection;

namespace Keycloak.Net.User.Apis;

public static class DependencyInjection
{
    public static IServiceCollection AddKeycloakApi(this IServiceCollection services)
    {
        services.AddHttpClient("api");
        services.AddScoped<IRequestUrlBuilder, RequestUrlBuilder>();
        services.AddScoped<IGetClientTokenQuery, GetClientTokenQuery>();
        services.AddScoped<IGetClientIdQuery, GetClientIdQuery>();
        services.AddScoped<IGetClientRoleQuery, GetClientRoleQuery>();
        services.AddScoped<IGetRealmRoleQuery, GetRealmRoleQuery>();
        services.AddScoped<IAssignUserRoleCommand, AssignUserRoleCommand>();
        return services;
    }
}
