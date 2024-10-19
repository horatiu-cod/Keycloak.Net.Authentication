using FluentAssertions;
using Keycloak.Net.User.Apis.Tests.Integration.Abstraction;

namespace Keycloak.Net.User.Apis.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class GetUserTokenQueryTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)
{
    [Fact]
    public async Task LoginUserAsync_ShouldReturnResultOK_WhenUserSuccessLoginToPublicClient()
    {
        //Arrange
        var clientId = "frontend";
        var UserName = "hg@g.com";
        var UserPassword = "s3cr3t";

        //Act
        var response = await _getUserTokenQuery.LoginUserAsync(clientId, UserName, UserPassword);

        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task LoginUserAsync_ShouldReturnResultUnauthorize_WhenUserFailLoginToPublicClient()
    {
        //Arrange
        var clientId = "frontend";
        var UserName = "hg@g.com";
        var UserPassword = "s3cr3te";

        //Act
        var response = await _getUserTokenQuery.LoginUserAsync( clientId, UserName, UserPassword);

        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task LoginUserAsync_ShouldReturnResultOK_WhenUserSuccessLoginToPrivateClient()
    {
        //Arrange
        var clientId = "backend";
        var clientSecret = "EO5W2VyNqDXAnTXknSS8t9a52qUzfmNy";
        const string UserName = "hg@g.com";
        const string UserPassword = "s3cr3t";

        //Act
        var response = await _getUserTokenQuery.LoginUserAsync(clientId, clientSecret, UserName, UserPassword);

        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task LoginUserAsync_ShouldReturnResultUnauthorized_WhenUserFailLoginToPrivateClient()
    {
        //Arrange
        var clientId = "backend";
        var clientSecret = "EO5W2VyNqDXAnTXknSS8t9a52qUzfmNy";
        const string UserName = "hg@g.com";
        const string UserPassword = "s3cr3t3";

        //Act
        var response = await _getUserTokenQuery.LoginUserAsync(clientId, clientSecret, UserName, UserPassword);

        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
}
