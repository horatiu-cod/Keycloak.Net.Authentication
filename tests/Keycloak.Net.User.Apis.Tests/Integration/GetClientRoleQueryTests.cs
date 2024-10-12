using FluentAssertions;
using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Role;
using Keycloak.Net.User.Apis.Features.Role.ClientRole;

namespace Keycloak.Net.User.Apis.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class GetClientRoleQueryTests(KeycloakFixture keycloakFixture)
{
    readonly IGetClientRoleQuery _clientRequest = new GetClientRoleQuery();
    readonly string url = BaseUrl.AdminUrl(keycloakFixture.BaseAddress?? string.Empty, "oidc");
    readonly HttpClient HttpClient = keycloakFixture.HttpClient;

    [Fact]
    public async Task GetClientRoleAsync_ShouldReturnRoleRepresentation_WhenRoleExists()
    {
        //Arrange
        var id = "973ef7f4-419f-4e77-87ae-ebfcfd714381";
        var roleName = "test";
        var role = new RoleRepresentation { Id = "8ea910b6-00b1-4243-9c8e-31ec5f5289bb", Name = "test" };

        //Act
        var response = await _clientRequest.GetClientRoleAsync(url, id, roleName, HttpClient);

        //Assert
        response.Content.Should().BeEquivalentTo(role);
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
    [Fact]
    public async Task GetClientRoleAsync_ShouldReturnNotFound_WhenRoleNotExists()
    {
        //Arrange
        var id = "973ef7f4-419f-4e77-87ae-ebfcfd714381";
        var roleName = "teste";// de schimbat
        var role = new RoleRepresentation { Id = "8ea910b6-00b1-4243-9c8e-31ec5f5289bb", Name = "test" };

        //Act
        var response = await _clientRequest.GetClientRoleAsync(url, id, roleName, HttpClient);

        //Assert
        response.Content.Should().BeNull();
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        response.Error.Should().Be("Could not find role");
    }

}
