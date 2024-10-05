using FluentAssertions;
using Keycloak.Net.User.Apis.Features.Role;
using Keycloak.Net.User.Apis.Features.Role.UserRole;

namespace Keycloak.Net.User.Apis.Tests.Integration;

public class AsignUserRoleCommandTests : IClassFixture<TokenRequestFixture>
{
    private readonly IAssignUserRoleCommand _sut;
    //readonly string _token;
    HttpClient _httpClient {  get; set; }
    readonly string url = "https://localhost:8843/admin/realms/oidc";
    public AsignUserRoleCommandTests(TokenRequestFixture tokenRequest)
    {
        _httpClient = tokenRequest._httpClient;
        _sut = new AssignUserRoleCommand();
    }
    [Fact]
    public async Task AssignClientRolesToUserAsync_ShouldReturnCreated_WhenClientRoleIsAssignedToUser()
    {
        //Assert
        var client = "16de1355-9764-44aa-a3b1-c427ccab4442";
        var uderId = "50ed8023-6c29-4b1a-b3da-62765c2a8d59";
        var role = new RoleRepresentation
        {
            Id = "79fd75d5-e08c-4054-8030-64509efeecbf",
            Name = "admin-role",
        };
        //Act
        var response = await _sut.AssignClientRolesToUserAsync(url, uderId, client, [role],  _httpClient);

        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }
    [Fact]
    public async Task AssignClientRolesToUserAsync_ShouldReturnNotFound_WhenClientRoleDoesNotExist()
    {
        //Assert
        var client = "16de1355-9764-44aa-a3b1-c427ccab4442";
        var uderId = "50ed8023-6c29-4b1a-b3da-62765c2a8d59";
        var role = new RoleRepresentation
        {
            Id = "79fd75d5-e08c-4054-8030-64509efeecb",
            Name = "admin-role",
        };
        //Act
        var response = await _sut.AssignClientRolesToUserAsync(url, uderId, client, [role], _httpClient);

        //Assert
        response.Error.Should().Be("Role not found");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

}
