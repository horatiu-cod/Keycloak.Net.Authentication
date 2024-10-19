using FluentAssertions;
using FluentAssertions.Execution;
using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Tests.Integration.Abstraction;

namespace Keycloak.Net.User.Apis.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class RegisterUserCommandTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)
{
    [Fact]
    public async Task RegisterUserAsync_ShouldReturnCreated_WhenUserRegisteredSuccessfully()
    {
        //Arrange
        var username = "cc@g.com";
        var password = "password";
        string email = "cc@g.com";
        string firstName = "c";
        string lastName = "g";

        var url = BaseUrl.AdminUrl(_baseAddress, _realmName);
        var clientName = "frontend";



        //Act
        var response = await _registerUserCommand.RegisterUser( username, password, email, firstName, lastName);
        var user = await _getUserIdQuery.GetUserIdAsync(url, username, _httpClient);
        var userToken = await _getUserTokenQuery.LoginUserAsync(clientName, username, password);
        //assert
        using (new AssertionScope())
        {
            response.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            user.IsSuccess.Should().BeTrue();
            userToken.IsSuccess.Should().BeTrue();
        }
    }
}
