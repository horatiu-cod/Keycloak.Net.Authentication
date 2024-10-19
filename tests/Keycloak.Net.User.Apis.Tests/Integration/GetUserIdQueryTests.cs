using FluentAssertions.Execution;
using FluentAssertions;
using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Tests.Integration.Abstraction;

namespace Keycloak.Net.User.Apis.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class GetUserIdQueryTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)
{
    [Fact]
    public async Task GetUserIdAsync_ShouldReturnUserId_WhenUserExist()
    {
        //Assert
        string url = BaseUrl.AdminUrl(_baseAddress, _realmName);
        var userName = "hg@g.com";
        var expectedUserId = "325ff607-8fe2-46bf-94fc-e0471a00ec70";

        //Act
        var response = await _getUserIdQuery.GetUserIdAsync(url, userName, _httpClient);

        //Assert
        using (new AssertionScope())
        {
            response.Content.Should().Be(expectedUserId);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }

    [Fact]
    public async Task GetUserIdAsync_ShouldReturnNotFound_WhenUserDoNotExist()
    {
        //Assert
        string url = BaseUrl.AdminUrl(_baseAddress, _realmName);
        var userName = "hgg@g.com";

        //Act
        var response = await _getUserIdQuery.GetUserIdAsync(url, userName, _httpClient);

        //Assert
        using (new AssertionScope())
        {
            response.Content.Should().BeNullOrEmpty();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
