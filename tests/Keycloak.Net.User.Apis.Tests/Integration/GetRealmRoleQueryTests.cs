using FluentAssertions;
using FluentAssertions.Execution;
using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Role;
using Keycloak.Net.User.Apis.Tests.Integration.Abstraction;

namespace Keycloak.Net.User.Apis.Tests.Integration;

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
        using(new AssertionScope()){
            response.Content.Should().BeEquivalentTo(role);
            response.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
 }
