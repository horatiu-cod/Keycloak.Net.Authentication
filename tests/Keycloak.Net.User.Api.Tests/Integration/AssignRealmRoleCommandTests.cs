using FluentAssertions;
using FluentAssertions.Execution;
using Keycloak.Net.User.Api.Tests.Integration.Abstraction;
using Keycloak.Net.User.Api.Features.Role;

namespace Keycloak.Net.User.Api.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class AssignRealmRoleCommandTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)//(KeycloakFixture keycloakFixture)
{
    [Fact]
    public async Task Handler_ShouldReturnNoContent_WhenRealmRoleIsAssignedToUser()
    {
        //Assert
        var userName = "temp@g.com";
        var roleName = "realm-test";
        var role = new RoleRepresentation
        {
            Id = "c59ec3a5-b01b-411e-8da1-d1b9787e27f8",
            Name = "realm-test",
        };
        //Act
        var response = await _assignRealmRoleCommand.Handler(userName, [roleName]);

        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Handler_ShouldReturnNotFound_WhenRealmRoleDoesNotExist()
    {
        //Assert
        var userName = "temp@g.com";
        var roleName = "realm-tes";
        var role = new RoleRepresentation
        {
            Id = "c59ec3a5-b01b-411e-8da1-d1b9787e27f8",
            Name = "realm-tes",
        };
        //Act
        var response = await _assignRealmRoleCommand.Handler(userName, [roleName]);

        //Assert
        using (new AssertionScope())
        {
            response.Error.Should().Be("Role not found");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }

}
