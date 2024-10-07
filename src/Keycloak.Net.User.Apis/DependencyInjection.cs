using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;
using Keycloak.Net.User.Apis.Features.Client.ClientRequest;
using Keycloak.Net.User.Apis.Features.Role.ClientRole;
using Keycloak.Net.User.Apis.Features.Role.RealmRole;
using Keycloak.Net.User.Apis.Features.Role.UserRole;
using Keycloak.Net.User.Apis.Features.User;
using Keycloak.Net.User.Apis.Features.User.DeleteUser;
using Keycloak.Net.User.Apis.Features.User.LoginUser;
using Keycloak.Net.User.Apis.Features.User.LogoutUser;
using Keycloak.Net.User.Apis.Features.User.RegisterUser;
using Keycloak.Net.User.Apis.Features.User.ResetPassword;
using Keycloak.Net.User.Apis.Features.User.UpdateUser;
using Keycloak.Net.User.Apis.Features.User.UserRefreshToken;
using Microsoft.Extensions.DependencyInjection;

namespace Keycloak.Net.User.Apis;

public static class DependencyInjection
{
    public static IServiceCollection AddKeycloakApi(this IServiceCollection services)
    {
        services.AddHttpClient("kapi");
        services.AddScoped<IRequestUrlBuilder, RequestUrlBuilder>();
        services.AddScoped<IGetClientTokenQuery, GetClientTokenQuery>();
        services.AddScoped<IGetClientIdQuery, GetClientIdQuery>();
        services.AddScoped<IGetClientRoleQuery, GetClientRoleQuery>();
        services.AddScoped<IGetRealmRoleQuery, GetRealmRoleQuery>();
        services.AddScoped<IAssignUserRoleCommand, AssignUserRoleCommand>();
        services.AddScoped<IDeleteUserCommand, DeleteUserCommand>();
        services.AddScoped<IGetUserTokenQuery, GetUserTokenQuery>();       
        services.AddScoped<ILogoutUserCommand, LogoutUserCommand>();
        services.AddScoped<IRegisterUserCommand, RegisterUserCommand>();
        services.AddScoped<IResetPasswordCommand, ResetPasswordCommand>();
        services.AddScoped<IUpdateUserCommand, UpdateUserCommand>();
        services.AddScoped<IUserRefreshTokenQuery, UserRefreshTokenQuery>();
        services.AddScoped<IGetUserQuery, GetUserQuery>();

        return services;
    }
}
