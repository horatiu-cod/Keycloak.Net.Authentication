using FluentAssertions;
using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Role;
using Keycloak.Net.User.Apis.Features.Role.RealmRole;

namespace Keycloak.Net.User.Apis.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class GetRealmRoleQueryTests(KeycloakFixture keycloakFixture) 
{
    readonly IGetRealmRoleQuery _sut = new GetRealmRoleQuery();
    readonly string url = BaseUrl.AdminUrl(keycloakFixture.BaseAddress?? string.Empty, "oidc");
    readonly HttpClient HttpClient = keycloakFixture.HttpClient;

    [Fact]
    public async Task GetRealmRoleAsync_WhenExists_ShouldReturnRoleRepresentation()
    {
        //Arrange
        var roleName = "admin-role";
        var role = new RoleRepresentation { Id = "12db6f22-57f9-499b-8f29-d64e4214f350", Name = "admin-role" };

        //Act
        var response = await _sut.GetRealmRoleAsync(url, roleName, HttpClient);

        //Assert
        response.Content.Should().BeEquivalentTo(role);
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
 }
