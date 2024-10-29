using FluentAssertions;
using FluentAssertions.Execution;
using Keycloak.Net.User.Api.Tests.Integration.Abstraction;

namespace Keycloak.Net.User.Api.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class UpdateUserCommandTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)
{
    [Fact]
    public async Task UpdateUserAsync_ShouldReturnNoContent_WhenUserUpdateSuccessfully()
    {
        //Arrange
        string username = "up@g.com";
        string firstName = "Hori";
        string lastName = string.Empty;
        string email = "";
        //Act
        var response = await _updateUserCommand.UpdateUserAsync(username, firstName, lastName, email);
        //Assert
        using (new AssertionScope())
        {
            response.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
    }
}
