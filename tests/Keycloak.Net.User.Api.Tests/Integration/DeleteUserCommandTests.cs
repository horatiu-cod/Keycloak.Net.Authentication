using FluentAssertions;
using FluentAssertions.Execution;
using Keycloak.Net.User.Api.Tests.Integration.Abstraction;
using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class DeleteUserCommandTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)
{

    [Fact]
    public async Task DeleteUserAsync_ShouldReturnNoContent_WhenUserDeleted()
    {
        //Arrange
        var userName = "del@g.com";
        var url = BaseUrl.AdminUrl(_baseAddress, _realmName);

        //Act
        var userId = await _getUserIdQuery.GetUserIdAsync(url, userName, _httpClient);
        var response = await _deleteUserCommand.Handler(userName, _clientId, _clientSecret);
        var user = await _getUserQuery.Handler(userName);
        //Assert
        using (new AssertionScope())
        {
            userId.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
            response.IsSuccess.Should().BeTrue();
            user.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
