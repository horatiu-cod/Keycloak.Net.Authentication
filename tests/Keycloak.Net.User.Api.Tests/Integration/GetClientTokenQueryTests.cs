using Keycloak.Net.User.Api.Features.Client.ClientAccessToken;
using System.Net;
using FluentAssertions;
using Keycloak.Net.User.Api.Common;
using FluentAssertions.Execution;
using Keycloak.Net.User.Api.Tests.Integration.Abstraction;

namespace Keycloak.Net.User.Api.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class GetClientTokenQueryTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)
{
    [Fact]
    public async Task GetClientTokenAsync_WhenConfidentialClientWitValidClientCredentials_ShouldReturnResultOK()
    {
        //Arrange
        string url = BaseUrl.TokenUrl(_baseAddress, _realmName);
        var httpClient = _httpClientFactory.CreateClient();
        var client = new GetClientTokenRequest(_clientId, _clientSecret);

        //Act
        var response = await _getClientTokenQuery.GetClientTokenAsync(url, client, httpClient);

        //Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNull();
        }
    }
    [Fact]
    public async Task GetClientTokenAsync_WhenConfidentialClientWithNotValidClientCredentials_ShouldReturnResultForbidden()
    {
        //Arrange
        string url = BaseUrl.TokenUrl(_baseAddress, _realmName);
        var httpClient = _httpClientFactory.CreateClient();

        var clientSecret = "2bpVgqGkUwUuagkJZ1DLK5Ncb3fkO1r";
        var client = new GetClientTokenRequest(_clientId, clientSecret);

        //Act
        var response = await _getClientTokenQuery.GetClientTokenAsync(url, client, httpClient);

        //Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            response.Content.Should().BeNull();
        }
    }
    [Fact]
    public async Task GetClientTokenAsync_WhenPublicClientWitValidClientName_ShouldReturnResultOK()
    {
        string url = BaseUrl.TokenUrl(_baseAddress, _realmName);
        var httpClient = _httpClientFactory.CreateClient();

        var clientSecret = string.Empty;
        var client = new GetClientTokenRequest(_clientId, clientSecret);

        //Act
        var response = await _getClientTokenQuery.GetClientTokenAsync(url, client, httpClient);

        //Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            response.Content.Should().BeNull();
        }
    }
}
