using FluentAssertions;
using FluentAssertions.Execution;
using Keycloak.Net.User.Api.Tests.Integration.Abstraction;
using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Features.Role;

namespace Keycloak.Net.User.Api.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class AssignRealmRoleInternalCommandTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)
{
    [Fact]
    public async Task AssignRealmRolesAsync_ShouldReturnNoContent_WhenRealmRoleIsAssignedToUser()
    {
        //Assert
        var uderId = "2d157a38-da3d-4fb8-84e1-de6677710222";
        var role = new RoleRepresentation
        {
            Id = "c59ec3a5-b01b-411e-8da1-d1b9787e27f8",
            Name = "realm-test",
        };
        //Act
        var response = await _assignRealmRoleCommandInternal.AssignRealmRolesAsync(BaseUrl.AdminUrl(_baseAddress, _realmName), uderId, [role], _httpClient);

        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task AssignRealmRolesAsync_ShouldReturnNotFound_WhenRealmRoleDoesNotExist()
    {
        //Assert
        var uderId = "2d157a38-da3d-4fb8-84e1-de6677710222";
        var role = new RoleRepresentation
        {
            Id = "c59ec3a5-b01b-411e-8da1-d1b9787e27f8",
            Name = "realm-tes",
        };
        //Act
        var response = await _assignRealmRoleCommandInternal.AssignRealmRolesAsync(BaseUrl.AdminUrl(_baseAddress, _realmName), uderId, [role], _httpClient);

        //Assert
        using (new AssertionScope())
        {
            response.Error.Should().Be("Role not found");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
