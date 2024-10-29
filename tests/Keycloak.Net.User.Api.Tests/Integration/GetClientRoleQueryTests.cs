using FluentAssertions;
using FluentAssertions.Execution;
using Keycloak.Net.User.Api.Tests.Integration.Abstraction;
using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Features.Role;

namespace Keycloak.Net.User.Api.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class GetClientRoleQueryTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)
{
    [Fact]
    public async Task GetClientRoleAsync_ShouldReturnRoleRepresentation_WhenRoleExists()
    {
        //Arrange
        string url = BaseUrl.AdminUrl(_baseAddress, _realmName);
        var id = "973ef7f4-419f-4e77-87ae-ebfcfd714381";
        var roleName = "test";
        var role = new RoleRepresentation { Id = "8ea910b6-00b1-4243-9c8e-31ec5f5289bb", Name = "test" };

        //Act
        var response = await _getClientRoleQuery.GetClientRoleAsync(url, id, roleName, _httpClient);

        //Assert
        using (new AssertionScope())
        {
            response.Content.Should().BeEquivalentTo(role);
            response.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        response.Content.Should().BeEquivalentTo(role);
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
    [Fact]
    public async Task GetClientRoleAsync_ShouldReturnNotFound_WhenRoleNotExists()
    {
        //Arrange
        string url = BaseUrl.AdminUrl(_baseAddress, _realmName);
        var id = "973ef7f4-419f-4e77-87ae-ebfcfd714381";
        var roleName = "teste";// de schimbat

        //Act
        var response = await _getClientRoleQuery.GetClientRoleAsync(url, id, roleName, _httpClient);

        //Assert
        using (new AssertionScope())
        {
            response.Content.Should().BeNull();
            response.IsSuccess.Should().BeFalse();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            response.Error.Should().Be("Could not find role");
        }
    }

}
