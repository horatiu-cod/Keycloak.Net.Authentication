using Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;
using Keycloak.Net.User.Apis.Features.Client.ClientRequest;
using Keycloak.Net.User.Apis.Features.Role.ClientRole;
using Keycloak.Net.User.Apis.Features.Role.RealmRole;
using Keycloak.Net.User.Apis.Features.Role.UserRole;
using Keycloak.Net.User.Apis.Features.User.AssignRole;
using Keycloak.Net.User.Apis.Features.User.Delete;
using Keycloak.Net.User.Apis.Features.User.Get;
using Keycloak.Net.User.Apis.Features.User.Login;
using Keycloak.Net.User.Apis.Features.User.Logout;
using Keycloak.Net.User.Apis.Features.User.RefreshToken;
using Keycloak.Net.User.Apis.Features.User.Register;
using Keycloak.Net.User.Apis.Features.User.ResetPassword;
using Keycloak.Net.User.Apis.Features.User.Update;
using Microsoft.Extensions.DependencyInjection;

namespace Keycloak.Net.User.Apis.Tests.Integration.Abstraction;

[Collection(nameof(KeycloakCollection))]
public abstract class BaseIntegrationTest
{
    private readonly IServiceScope _scope;
    internal readonly IGetClientTokenQuery _getClientTokenQuery;
    internal readonly IGetClientIdQuery _getClientIdQuery;
    internal readonly IGetClientRoleQuery _getClientRoleQuery;
    internal readonly IGetRealmRoleQuery _getRealmRoleQuery;
    internal readonly IAssignRealmRoleInternalCommand _assignRealmRoleCommandInternal;
    internal readonly IAssignClientRoleInternalCommand _assignClientRoleCommandInternal;
    internal readonly IAssignClientRoleCommand _assignClientRoleCommand;
    internal readonly IAssignRealmRoleCommand _assignRealmRoleCommand;
    internal readonly IDeleteUserCommand _deleteUserCommand;
    internal readonly IGetUserTokenQuery _getUserTokenQuery;
    internal readonly ILogoutUserCommand _logoutUserCommand;
    internal readonly IRegisterUserCommand _registerUserCommand;
    internal readonly IRegisterUserWithClientRoleCommand _registerUserWithClientRoleCommand;
    internal readonly IRegisterUserWithRealmRoleCommand _registerUserWithRealmRoleCommand;
    internal readonly IResetPasswordCommand _resetPasswordCommand;
    internal readonly IUpdateUserCommand _updateUserCommand;
    internal readonly IUserRefreshTokenQuery _userRefreshTokenQuery;
    internal readonly IGetUserIdQuery _getUserIdQuery;
    internal readonly IGetUserQuery _getUserQuery;
    internal readonly IGetUserQueryInternal _getUserQueryInternal;


    public readonly IHttpClientFactory _httpClientFactory;

    public readonly string _baseAddress;
    public readonly string _realmName;
    public HttpClient _httpClient;
    public readonly string _clientId;
    public readonly string _clientSecret;


    protected BaseIntegrationTest(KeycloakFixture keycloakFixture)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddKeycloakApi(s=>
        {
            s.BaseAddress = keycloakFixture.BaseAddress;
            s.RealmName = keycloakFixture.realmName;
        }).AdminClient(keycloakFixture.clientId, keycloakFixture.clientSecret);
        services.AddHttpClient();
        _scope = services.BuildServiceProvider().CreateScope();
        _getClientTokenQuery = _scope.ServiceProvider.GetRequiredService<IGetClientTokenQuery>();
        _getClientIdQuery = _scope.ServiceProvider.GetRequiredService<IGetClientIdQuery>();
        _getClientRoleQuery = _scope.ServiceProvider.GetRequiredService<IGetClientRoleQuery>();
        _getRealmRoleQuery = _scope.ServiceProvider.GetRequiredService<IGetRealmRoleQuery>();
        _assignRealmRoleCommandInternal = _scope.ServiceProvider.GetRequiredService<IAssignRealmRoleInternalCommand>();
        _assignRealmRoleCommand = _scope.ServiceProvider.GetRequiredService<IAssignRealmRoleCommand>();
        _assignClientRoleCommandInternal = _scope.ServiceProvider.GetRequiredService<IAssignClientRoleInternalCommand>();
        _assignClientRoleCommand = _scope.ServiceProvider.GetRequiredService<IAssignClientRoleCommand>();
        _deleteUserCommand = _scope.ServiceProvider.GetRequiredService<IDeleteUserCommand>();
        _getUserTokenQuery = _scope.ServiceProvider.GetRequiredService<IGetUserTokenQuery>();
        _logoutUserCommand = _scope.ServiceProvider.GetRequiredService<ILogoutUserCommand>();
        _registerUserCommand = _scope.ServiceProvider.GetRequiredService<IRegisterUserCommand>();
        _registerUserWithClientRoleCommand = _scope.ServiceProvider.GetRequiredService<IRegisterUserWithClientRoleCommand>();
        _registerUserWithRealmRoleCommand = _scope.ServiceProvider.GetRequiredService<IRegisterUserWithRealmRoleCommand>();
        _resetPasswordCommand = _scope.ServiceProvider.GetRequiredService<IResetPasswordCommand>();
        _updateUserCommand = _scope.ServiceProvider.GetRequiredService<IUpdateUserCommand>();
        _userRefreshTokenQuery = _scope.ServiceProvider.GetRequiredService<IUserRefreshTokenQuery>();
        _getUserIdQuery = _scope.ServiceProvider.GetRequiredService<IGetUserIdQuery>();
        _getUserQuery = _scope.ServiceProvider.GetRequiredService<IGetUserQuery>();
        _getUserQueryInternal = _scope.ServiceProvider.GetRequiredService<IGetUserQueryInternal>();

        _httpClientFactory = services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>();

        _baseAddress = keycloakFixture.BaseAddress;
        _realmName = keycloakFixture.realmName;
        _httpClient = keycloakFixture.HttpClient;
        _clientId = keycloakFixture.clientId;
        _clientSecret = keycloakFixture.clientSecret;
    }
}
