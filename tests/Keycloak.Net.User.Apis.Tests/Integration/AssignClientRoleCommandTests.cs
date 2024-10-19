using FluentAssertions;
using FluentAssertions.Execution;
using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Role;
using Keycloak.Net.User.Apis.Tests.Integration.Abstraction;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
namespace Keycloak.Net.User.Apis.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class AssignClientRoleCommandTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)
{
    [Fact]
    public async Task Handler_ShouldReturnNoContent_WhenClientRoleIsAssignedToUser()
    {
        //Assert
        var client = "frontend";
        var userName = "temp@g.com";
        var roleName = "test";
        var role = new RoleRepresentation
        {
            Id = "8ea910b6-00b1-4243-9c8e-31ec5f5289bb",
            Name = "test",
        };
        //Act
        var response = await _assignClientRoleCommand.Handler( userName, client, [roleName]);

        var userToken = await _getUserTokenQuery.LoginUserAsync(client, userName, "s3cr3t");
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userToken.Content?.AccessToken);

        var url = $"{BaseUrl.RealmUrl(_baseAddress, _realmName)}/protocol/openid-connect/userinfo";
        var userInfo = await httpClient.GetAsync(url);
        var content = await userInfo.Content.ReadFromJsonAsync<JsonObject>();
        var roleExist = content?.Root["resource_access"]?[$"{client}"]?["roles"]?.AsArray().Any(c => c?.GetValue<string>() == "test");

        //Assert
        using(new AssertionScope())
        {
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
            roleExist.Should().BeTrue();
        }
    }
    [Fact]
    public async Task Handler_ShouldReturnNotFound_WhenClientRoleDoesNotExist()
    {
        //Assert
        var client = "frontend";
        var userName = "temp@g.com";
        var roleName = "tes";
        //Act
        var response = await _assignClientRoleCommand.Handler(userName, client, [roleName]);

        //Assert
        using (new AssertionScope())
        {
            response.Error.Should().Be("Could not find role");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
