using Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;
using System.Net;
using FluentAssertions;
using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class GetClientTokenQueryTests(KeycloakFixture keycloakFixture)
{
    private readonly IGetClientTokenQuery _clientTokenRequest = new GetClientTokenQuery();
    readonly string url = BaseUrl.TokenUrl(keycloakFixture.BaseAddress?? string.Empty, "oidc");
    readonly HttpClient HttpClient = new();  

    [Fact]
    public async Task GetClientTokenAsync_WhenValidClientCredentials_ShouldReturnResultOK()
    {
        //Arrange
        var clientId = "management";
        var clientSecret = "2bpVgqGkUwUuagkJZ1DLK5Ncb3fkO1ri";
        var client = new GetClientTokenRequest(clientId, clientSecret);

        //Act
        var response = await _clientTokenRequest.GetClientTokenAsync(url, client, HttpClient);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
    }
    [Fact]
    public async Task GetClientTokenAsync_WhenNotValidClientCredentials_ShouldReturnResultForbidden()
    {
        //Arrange
        var clientId = "management";
        var clientSecret = "2bpVgqGkUwUuagkJZ1DLK5Ncb3fkO1r";
        var client = new GetClientTokenRequest(clientId, clientSecret);

        //Act
        var response = await _clientTokenRequest.GetClientTokenAsync(url, client, HttpClient);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.Content.Should().BeNull();
    }
}
