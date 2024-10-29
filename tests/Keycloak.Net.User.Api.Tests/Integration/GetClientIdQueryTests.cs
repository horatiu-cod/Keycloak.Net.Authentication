using FluentAssertions;
using FluentAssertions.Execution;
using Keycloak.Net.User.Api.Tests.Integration.Abstraction;
using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Features.Client.ClientRequest;

namespace Keycloak.Net.User.Api.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class GetClientIdQueryTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)
{
    [Fact]
    public async Task GetClientAsync_ShouldReturnClientId_WhenCredentialsAreValid()
    {
        //Arrange
        string url = BaseUrl.AdminUrl(_baseAddress, _realmName);
        var clientId = "frontend";
        var id = "973ef7f4-419f-4e77-87ae-ebfcfd714381";
        var expectedClientResponse = new GetClientIdResponse(id);

        //Act
        var response = await _getClientIdQuery.GetClientIdAsync(url, clientId, _httpClient);

        //Assert
        using (new AssertionScope())
        {
            response.Content.Should().BeEquivalentTo(expectedClientResponse);
            response.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
    [Fact]
    public async Task GetClientAsync_ShouldReturnClientNotFound_WhenCredentialsAreNotValid()
    {
        //Arrange
        string url = BaseUrl.AdminUrl(_baseAddress, _realmName);
        var clientId = "frontende";

        //Act
        var response = await _getClientIdQuery.GetClientIdAsync(url, clientId, _httpClient);

        //Assert
        using (new AssertionScope())
        {
            response.Content.Should().BeNull();
            response.IsSuccess.Should().BeFalse();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
