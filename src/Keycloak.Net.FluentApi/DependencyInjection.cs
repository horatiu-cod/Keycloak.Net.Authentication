using Keycloak.Net.FluentApi.Features.Client;
using Keycloak.Net.FluentApi.Features.Client.ClientAccessToken;
using Keycloak.Net.FluentApi.Features.Role.ClientRole;
using Keycloak.Net.FluentApi.Features.Role.RealmRole;
using Keycloak.Net.FluentApi.Features.User;
using Keycloak.Net.FluentApi.Features.User.RegisterUser;
using Keycloak.Net.FluentApi.Features.User.UserRole;
using Microsoft.Extensions.DependencyInjection;

namespace Keycloak.Net.FluentApi;

internal static class DependencyInjection
{
    public static IServiceCollection AddKeycloakFluentApi(this IServiceCollection services)
    {
        services.AddScoped<IClientTokenRequest, ClientTokenRequest>();
        services.AddScoped<IClientRequest, ClientRequest>();
        services.AddScoped<IClientRoleRequest, ClientRoleRequest>();
        services.AddScoped<IRealmRoleRequest, RealmRoleRequest>();
        services.AddScoped<IRegisterUserCommand, RegisterUserCommand>();
        services.AddScoped<IUserClientRoleRequest, UserClientRoleRequest>();
        services.AddScoped<IUserRealmRoleRequest, UserRealmRoleRequest>();
        services.AddScoped<IUserRequest, UserRequest>();
        services.AddScoped<IRegisterUser, RegisterUser>();

        return services;
    }
}
