using FluentAssertions;
using FluentAssertions.Execution;
using Keycloak.Net.User.Api.Tests.Integration.Abstraction;

namespace Keycloak.Net.User.Api.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class UserRefreshTokenQueryTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)
{
    [Fact]
    public async Task RefreshTokenAsync_ShouldReturnTokenRepresentation_WhenRefreshTokenSucceed()
    {
        //Arrange
        var clientId = "frontend";
        var UserId = "hg@g.com";
        var UserPassword = "s3cr3t";

        //Act
        var token = await _getUserTokenQuery.LoginUserAsync(clientId, UserId, UserPassword);
        var response = await _userRefreshTokenQuery.RefreshTokenAsync(clientId, string.Empty, token.Content?.RefreshToken!);

        //Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Content?.AccessToken.Should().NotBeNullOrEmpty();
        }
    }
}
