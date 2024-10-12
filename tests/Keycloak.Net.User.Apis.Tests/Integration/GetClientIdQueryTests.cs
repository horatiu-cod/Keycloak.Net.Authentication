using FluentAssertions;
using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Client.ClientRequest;

namespace Keycloak.Net.User.Apis.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class GetClientIdQueryTests(KeycloakFixture keycloakFixture)
{
    private readonly IGetClientIdQuery _sut = new GetClientIdQuery();
    readonly HttpClient _httpClient = keycloakFixture.HttpClient;
    readonly string url = BaseUrl.AdminUrl(keycloakFixture.BaseAddress?? string.Empty, "oidc");

    [Fact]
    public async Task GetClientAsync_ShouldReturnClientId_WhenCredentialsAreValid()
    {
        //Arrange
        var clientId = "frontend";
        var id = "973ef7f4-419f-4e77-87ae-ebfcfd714381";
        var expectedClientResponse = new GetClientIdResponse(id);
          
        //Act
        var response = await _sut.GetClientAsync(url, clientId,  _httpClient);

        //Assert
        response.Content.Should().BeEquivalentTo(expectedClientResponse);
        response.IsSuccess.Should().BeTrue();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
    [Fact]
    public async Task GetClientAsync_ShouldReturnClientNotFound_WhenCredentialsAreNotValid()
    {
        //Arrange
        var clientId = "frontende";

        //Act
        var response = await _sut.GetClientAsync(url, clientId, _httpClient);

        //Assert
        response.Content.Should().BeNull();
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

}
