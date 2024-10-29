using Keycloak.Net.User.Api.Configuration;
using Keycloak.Net.User.Api.Features.Client.ClientAccessToken;
using Keycloak.Net.User.Api.Features.Client.ClientRequest;
using Keycloak.Net.User.Api.Features.Role.ClientRole;
using Keycloak.Net.User.Api.Features.Role.RealmRole;
using Keycloak.Net.User.Api.Features.Role.UserRole;
using Keycloak.Net.User.Api.Features.User.AssignRole;
using Keycloak.Net.User.Api.Features.User.Delete;
using Keycloak.Net.User.Api.Features.User.Get;
using Keycloak.Net.User.Api.Features.User.Login;
using Keycloak.Net.User.Api.Features.User.Logout;
using Keycloak.Net.User.Api.Features.User.RefreshToken;
using Keycloak.Net.User.Api.Features.User.Register;
using Keycloak.Net.User.Api.Features.User.ResetPassword;
using Keycloak.Net.User.Api.Features.User.Update;
using Microsoft.Extensions.Logging;

namespace Keycloak.Net.User.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddKeycloakApi(this IServiceCollection services, Action<Server> ServerConfiguration)
    {
        services.AddOptionsWithValidateOnStart<Server>().Configure(ServerConfiguration).Validate(s =>
        {
            if (s.BaseAddress is null && s.RealmName is null)
                return false;
            return true;
        });
        services.AddHttpClient("kapi");
        // TODO de verificat
        //services.AddHttpClient("kapi", (services, client) =>
        //{
        //    var server = services.GetRequiredService<IOptions<Server>>().Value;
        //    client.BaseAddress = new Uri(server.BaseAddress);
        //});

        services.AddLogging(static builder =>
        {
            builder.AddFilter(c => c == LogLevel.Information).AddConsole().AddDebug();
        });

        services.AddScoped<IGetClientTokenQuery, GetClientTokenQuery>();
        services.AddScoped<IGetClientIdQuery, GetClientIdQuery>();
        services.AddScoped<IGetClientRoleQuery, GetClientRoleQuery>();
        services.AddScoped<IGetRealmRoleQuery, GetRealmRoleQuery>();
        services.AddScoped<IAssignRealmRoleInternalCommand, AssignRealmRoleInternalCommand>();
        services.AddScoped<IAssignClientRoleInternalCommand, AssignClientRoleInternalCommand>();
        services.AddScoped<IAssignRealmRoleCommand, AssignRealmRoleCommand>();
        services.AddScoped<IAssignClientRoleCommand, AssignClientRoleCommand>();
        services.AddScoped<IDeleteUserCommand, DeleteUserCommand>();
        services.AddScoped<IGetUserTokenQuery, GetUserTokenQuery>();
        services.AddScoped<ILogoutUserCommand, LogoutUserCommand>();
        services.AddScoped<IRegisterUserCommand, RegisterUserCommand>();
        services.AddScoped<IRegisterUserWithClientRoleCommand, RegisterUserWithClientRoleCommand>();
        services.AddScoped<IRegisterUserWithRealmRoleCommand, RegisterUserWithRealmRoleCommand>();
        services.AddScoped<IResetPasswordCommand, ResetPasswordCommand>();
        services.AddScoped<IUpdateUserCommand, UpdateUserCommand>();
        services.AddScoped<IUserRefreshTokenQuery, UserRefreshTokenQuery>();
        services.AddScoped<IGetUserIdQuery, GetUserIdQuery>();
        services.AddScoped<IGetUserQuery, GetUserQuery>();
        services.AddScoped<IGetUserQueryInternal, GetUserQueryInternal>();

        return services;
    }
    public static IServiceCollection AdminClient(this IServiceCollection services, string clientId, string clientSecret)
    {
        services.AddOptionsWithValidateOnStart<AdminClient>().Configure(c =>
        {
            c.ClientId = clientId;
            c.ClientSecret = clientSecret;
        }).Validate(c =>
        {
            if (c.ClientId is null && c.ClientSecret is null)
                return false;
            return true;
        });
        return services;
    }
    public static IServiceCollection AdminClient(this IServiceCollection services, Action<AdminClient> AdminClient)
    {
        services.AddOptionsWithValidateOnStart<AdminClient>().Configure(AdminClient).Validate(c =>
        {
            if (c.ClientId is null && c.ClientSecret is null)
                return false;
            return true;
        });
        return services;
    }
}
