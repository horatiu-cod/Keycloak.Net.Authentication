using Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;
using System.Net;
using FluentAssertions;
using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Tests.Integration;

public class GetClientTokenQueryTests
{
    private readonly HttpClient _httpClient;
    private readonly IGetClientTokenQuery _clientTokenRequest;
    readonly string realmName = "oidc";
    readonly string baseAddress = "https://localhost:8843";
    readonly string url;
    //const string url = "https://localhost:8843/realms/oidc/protocol/openid-connect/token"
    //string url = "https://localhost:8843/realms/oidc/protocol/openid-connect/token"

    public GetClientTokenQueryTests()
    {
        _clientTokenRequest = new GetClientTokenQuery();
        _httpClient = new HttpClient();
         url = BaseUrl.TokenUrl(baseAddress, realmName);

    }
    [Fact]
    public async Task GetClientTokenAsync_WhenValidClientCredentials_ShouldReturnResultOK()
    {
        //Arrange
        var clientId = "auth-client";
        var clientSecret = "JsCpqGIfQFWWO0dhUSjaNAnZGR4JhEHC";
        var client = new GetClientTokenRequest(clientId, clientSecret);

        //Act
        var response = await _clientTokenRequest.GetClientTokenAsync(url, client,_httpClient);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
    }
    [Fact]
    public async Task GetClientTokenAsync_WhenNotValidClientCredentials_ShouldReturnResultForbidden()
    {
        //Arrange
        var clientId = "auth-client";
        var clientSecret = "JsCpqGIfQFWWO0dhUSjaNAnZGR4JhEH";
        var client = new GetClientTokenRequest(clientId, clientSecret);

        //Act
        var response = await _clientTokenRequest.GetClientTokenAsync(url, client, _httpClient);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.Content.Should().BeNull();
    }
}
