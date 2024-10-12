using FluentAssertions;
using Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;
using Keycloak.Net.User.Apis.Features.User;
using Keycloak.Net.User.Apis.Features.User.UpdateUser;
using Microsoft.Extensions.DependencyInjection;

namespace Keycloak.Net.User.Apis.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class UpdateUserCommandTests 
{
    
    readonly string realmName = "oidc";
    readonly string baseAddress;
    readonly IUpdateUserCommand _sut;
    const string clientId = "management";
    const string clientSecret = "2bpVgqGkUwUuagkJZ1DLK5Ncb3fkO1ri";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IGetClientTokenQuery _clientTokenQuery;
    private readonly IGetUserQuery _userQuery;

    public UpdateUserCommandTests(KeycloakFixture keycloakFixture)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddHttpClient();
        _httpClientFactory = services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>();
        _clientTokenQuery = new GetClientTokenQuery();
        _userQuery = new GetUserQuery();
        baseAddress = keycloakFixture.BaseAddress?? string.Empty;
        _sut = new UpdateUserCommand(_httpClientFactory, _clientTokenQuery, _userQuery);
    }
    [Fact]
    public async Task UpdateUserAsync_ShouldReturnNoContent_WhenUserUpdateSuccessfully()
    {
        //Arrange
        string username = "hg@g.com";
        string firstName = "Hori";
        string lastName = string.Empty;
        string email = "";
        //Act
        var response = await _sut.UpdateUserAsync(baseAddress,realmName, clientId, clientSecret, username, firstName, lastName, email);
        //Assert
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }
}
