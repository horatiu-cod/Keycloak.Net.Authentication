using FluentAssertions;
using Keycloak.Net.User.Apis.Features.Role;
using Keycloak.Net.User.Apis.Features.Role.ClientRole;

namespace Keycloak.Net.User.Apis.Tests.Integration;

public class GetClientRoleQueryTests : IClassFixture<TokenRequestFixture>
{
    readonly IGetClientRoleQuery _clientRequest;
    //readonly string _token;
    HttpClient _httpClient {  get; set; }
    const string url = "https://localhost:8843/admin/realms/oidc";

    public GetClientRoleQueryTests(TokenRequestFixture tokenRequest)
    {
        _httpClient = tokenRequest._httpClient;
        _clientRequest = new GetClientRoleQuery();
    }

    [Fact]
    public async Task GetClientRoleAsync_ShouldReturnRoleRepresentation_WhenRoleExists()
    {
        //Arrange
        var id = "16de1355-9764-44aa-a3b1-c427ccab4442";
        var roleName = "admin-role";
        var role = new RoleRepresentation { Id = "79fd75d5-e08c-4054-8030-64509efeecbf", Name = "admin-role" };

        //Act
        var response = await _clientRequest.GetClientRoleAsync(url, id, roleName, _httpClient);

        //Assert
        response.Content.Should().BeEquivalentTo(role);
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
    [Fact]
    public async Task GetClientRoleAsync_ShouldReturnNotFound_WhenRoleNotExists()
    {
        //Arrange
        var id = "16de1355-9764-44aa-a3b1-c427ccab4442";
        var roleName = "admin-rolel";
        var role = new RoleRepresentation { Id = "79fd75d5-e08c-4054-8030-64509efeecbf", Name = "admin-role" };

        //Act
        var response = await _clientRequest.GetClientRoleAsync(url, id, roleName, _httpClient);

        //Assert
        response.Content.Should().BeNull();
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        response.Error.Should().Be("Could not find role");
    }

}
