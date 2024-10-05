using FluentAssertions;
using Keycloak.Net.User.Apis.Features.Client.ClientRequest;
using Keycloak.Net.User.Apis.Features.Role;
using Keycloak.Net.User.Apis.Features.Role.RealmRole;

namespace Keycloak.Net.User.Apis.Tests.Integration;

public class GetRealmRoleQueryTests : IClassFixture<TokenRequestFixture>
{
    readonly IGetRealmRoleQuery _sut;
    //readonly string _token;
    HttpClient _httpClient { get; set; }
    const string url = "https://localhost:8843/admin/realms/oidc";

    public GetRealmRoleQueryTests(TokenRequestFixture tokenRequest)
    {
        _httpClient = tokenRequest._httpClient;
        //tokenRequest.GetTokenAsync(_httpClient);
        _sut = new GetRealmRoleQuery();
    }

    [Fact]
    public async Task GetRealmRoleAsync_WhenExists_ShouldReturnRoleRepresentation()
    {
        //Arrange
        var roleName = "admin-role";
        var role = new RoleRepresentation { Id = "b5fa98d0-1bf0-40a4-b4d1-0cf805c35b59", Name = "admin-role" };

        //Act
        var response = await _sut.GetRealmRoleAsync(url, roleName, _httpClient);

        //Assert
        response.Content.Should().BeEquivalentTo(role);
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
 }
