using FluentAssertions;
using FluentAssertions.Execution;
using Keycloak.Net.User.Api.Tests.Integration.Abstraction;
using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class RegisterUserCommandTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)
{
    [Fact]
    public async Task Handler_ShouldReturnCreated_WhenUserRegisteredSuccessfully()
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
        var response = await _registerUserCommand.Handler(username, password, email, firstName, lastName);
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
    [Fact]
    public async Task Handler_ShouldReturnConflict_WhenUserAlreadyRegistered()
    {
        //Arrange
        var username = "hg@g.com";
        var password = "password";
        string email = "hg@g.com";
        string firstName = "c";
        string lastName = "g";

        //Act
        var response = await _registerUserCommand.Handler(username, password, email, firstName, lastName);

        //assert
        using (new AssertionScope())
        {
            response.IsSuccess.Should().BeFalse();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
        }
    }
}
