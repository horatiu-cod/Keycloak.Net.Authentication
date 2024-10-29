using FluentAssertions;
using FluentAssertions.Execution;
using Keycloak.Net.User.Api.Tests.Integration.Abstraction;
using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Features.Role;

namespace Keycloak.Net.User.Api.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class AssignClientRoleInternalCommandTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)
{
    [Fact]
    public async Task AssignClientRolesAsync_ShouldReturnNoContent_WhenClientRoleIsAssignedToUser()
    {
        //Assert
        var client = "973ef7f4-419f-4e77-87ae-ebfcfd714381";
        var userId = "2d157a38-da3d-4fb8-84e1-de6677710222";
        var url = BaseUrl.AdminUrl(_baseAddress, _realmName);
        var role = new RoleRepresentation
        {
            Id = "8ea910b6-00b1-4243-9c8e-31ec5f5289bb",
            Name = "test",
        };
        //Act
        var response = await _assignClientRoleCommandInternal.AssignClientRolesAsync(url, userId, client, [role], _httpClient);

        //Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
    }

    [Fact]
    public async Task AssignClientRolesAsync_ShouldReturnNotFound_WhenClientRoleDoesNotExist()
    {
        //Assert
        var client = "973ef7f4-419f-4e77-87ae-ebfcfd714381";
        var userId = "2d157a38-da3d-4fb8-84e1-de6677710222";
        var url = BaseUrl.AdminUrl(_baseAddress, _realmName);
        var role = new RoleRepresentation
        {
            Id = "8ea910b6-00b1-4243-9c8e-31ec5f5289b",
            Name = "test",
        };
        //Act
        var response = await _assignClientRoleCommandInternal.AssignClientRolesAsync(url, userId, client, [role], _httpClient);

        //Assert
        using (new AssertionScope())
        {
            response.Error.Should().Be("Role not found");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
