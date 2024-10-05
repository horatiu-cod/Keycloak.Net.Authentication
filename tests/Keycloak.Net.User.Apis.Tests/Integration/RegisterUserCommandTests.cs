using FluentAssertions;
using Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;
using Keycloak.Net.User.Apis.Features.Role.ClientRole;
using Keycloak.Net.User.Apis.Features.Role.RealmRole;
using Keycloak.Net.User.Apis.Features.Role.UserRole;
using Keycloak.Net.User.Apis.Features.User;
using Keycloak.Net.User.Apis.Features.User.RegisterUser;
using Microsoft.Extensions.DependencyInjection;

namespace Keycloak.Net.User.Apis.Tests.Integration;

public class RegisterUserCommandTests
{
    readonly string realmName = "oidc";
    readonly string baseAddress = "https://localhost:8843";
    const string clientId = "auth-client";
    const string clientSecret = "JsCpqGIfQFWWO0dhUSjaNAnZGR4JhEHC";

    private readonly IRegisterUserCommand _sut;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IGetClientTokenQuery _clientTokenQuery;
    private readonly IGetUserQuery _userQuery;
    private readonly IAssignUserRoleCommand _assignUserRoleCommand;
    private readonly IGetRealmRoleQuery _realmRoleQuery;
    private readonly IGetClientRoleQuery _clientRoleQuery;

    public RegisterUserCommandTests()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddHttpClient();
        _httpClientFactory = services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>();
        _clientTokenQuery = new GetClientTokenQuery();
        _userQuery = new GetUserQuery();
        _assignUserRoleCommand = new AssignUserRoleCommand();
        _realmRoleQuery = new GetRealmRoleQuery();
        _clientRoleQuery = new GetClientRoleQuery();

        _sut = new RegisterUserCommand(_clientTokenQuery, _realmRoleQuery, _assignUserRoleCommand, _clientRoleQuery, _userQuery, _httpClientFactory);
    }
    [Fact]
    public async Task RegisterUserAsync_ShouldReturnCreated_WhenUserRegisteredSuccessfully()
    {
        //Arrange
        var username = "cc@g.com";
        var password = "password";

        //Act
        var response = await _sut.RegisterUser(baseAddress, realmName,clientId, clientSecret, username, password);
        //assert

        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }
}
