using FluentAssertions;
using FluentAssertions.Execution;
using Keycloak.Net.User.Api.Tests.Integration.Abstraction;

namespace Keycloak.Net.User.Api.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class ResetPasswordCommandTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)
{
    [Fact]
    public async Task ResetPasswordAsync_ShouldUpdatePassword_WhenUserCredentialsAreOk()
    {
        //Arrange
        var currentPassword = "s3cr3t";
        var username = "rp@g.com";
        var password = "s3cr3t3";
        var clientName = "frontend";

        //Act
        var responseToken = await _getUserTokenQuery.LoginUserAsync(clientName, username, currentPassword);

        var resetResponse = await _resetPasswordCommand.Handler(username, password);
        var response = await _getUserTokenQuery.LoginUserAsync(clientName, username, password);

        //Assert
        using (new AssertionScope())
        {
            responseToken.IsSuccess.Should().BeTrue();
            resetResponse.IsSuccess.Should().BeTrue();
            resetResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
            response.IsSuccess.Should().BeTrue();
        }
    }
}
