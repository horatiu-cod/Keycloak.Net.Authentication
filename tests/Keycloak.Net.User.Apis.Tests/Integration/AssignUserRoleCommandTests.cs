using FluentAssertions;
using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Role;
using Keycloak.Net.User.Apis.Features.Role.UserRole;

namespace Keycloak.Net.User.Apis.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class AssignUserRoleCommandTests(KeycloakFixture keycloakFixture)
{
    private readonly IAssignUserRoleCommand _sut = new AssignUserRoleCommand();
    readonly string realmName = "oidc";
    private readonly HttpClient httpClient = keycloakFixture.HttpClient;
    private readonly string baseAddress = keycloakFixture.BaseAddress?? string.Empty;

    [Fact]
    public async Task AssignClientRolesToUserAsync_ShouldReturnCreated_WhenClientRoleIsAssignedToUser()
    {
        //Assert
        var client = "973ef7f4-419f-4e77-87ae-ebfcfd714381";
        var uderId = "2d157a38-da3d-4fb8-84e1-de6677710222";
        var role = new RoleRepresentation
        {
            Id = "8ea910b6-00b1-4243-9c8e-31ec5f5289bb",
            Name = "test",
        };
        //Act
        var response = await _sut.AssignClientRolesToUserAsync(BaseUrl.AdminUrl(baseAddress, realmName), uderId, client, [role],  httpClient);

        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }
    [Fact]
    public async Task AssignClientRolesToUserAsync_ShouldReturnNotFound_WhenClientRoleDoesNotExist()
    {
        //Assert
        var client = "973ef7f4-419f-4e77-87ae-ebfcfd714381";
        var uderId = "2d157a38-da3d-4fb8-84e1-de6677710222";
        var role = new RoleRepresentation
        {
            Id = "8ea910b6-00b1-4243-9c8e-31ec5f5289b",// de schimbat
            Name = "test",
        };
        //Act
        var response = await _sut.AssignClientRolesToUserAsync(BaseUrl.AdminUrl(baseAddress, realmName), uderId, client, [role], httpClient);

        //Assert
        response.Error.Should().Be("Role not found");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

}
