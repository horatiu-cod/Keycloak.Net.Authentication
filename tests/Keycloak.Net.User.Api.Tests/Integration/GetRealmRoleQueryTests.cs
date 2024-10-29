using FluentAssertions;
using FluentAssertions.Execution;
using Keycloak.Net.User.Api.Tests.Integration.Abstraction;
using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Features.Role;

namespace Keycloak.Net.User.Api.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class GetRealmRoleQueryTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)
{
    [Fact]
    public async Task GetRealmRoleAsync_WhenExists_ShouldReturnRoleRepresentation()
    {
        //Arrange
        string url = BaseUrl.AdminUrl(_baseAddress, _realmName);
        var roleName = "admin-role";
        var role = new RoleRepresentation { Id = "12db6f22-57f9-499b-8f29-d64e4214f350", Name = "admin-role" };

        //Act
        var response = await _getRealmRoleQuery.GetRealmRoleAsync(url, roleName, _httpClient);

        //Assert
        using (new AssertionScope())
        {
            response.Content.Should().BeEquivalentTo(role);
            response.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
