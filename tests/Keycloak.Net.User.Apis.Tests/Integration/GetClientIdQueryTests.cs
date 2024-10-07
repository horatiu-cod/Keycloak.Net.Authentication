using FluentAssertions;
using Keycloak.Net.User.Apis.Features.Client.ClientRequest;

namespace Keycloak.Net.User.Apis.Tests.Integration;

public class GetClientIdQueryTests : IClassFixture<TokenRequestFixture>
{
    private readonly IGetClientIdQuery _sut;
    readonly HttpClient _httpClient;
    readonly string url = "https://localhost:8843/admin/realms/oidc";

    public GetClientIdQueryTests(TokenRequestFixture tokenRequest)
    {
        _httpClient = tokenRequest._httpClient;
        _sut = new GetClientIdQuery();
    }
    [Fact]
    public async Task GetClientAsync_ShouldReturnClientId_WhenCredentialsAreValid()
    {
        //Arrange
        var clientId = "public-client";
        var id = "16de1355-9764-44aa-a3b1-c427ccab4442";
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
        var clientId = "public-clientt";

        //Act
        var response = await _sut.GetClientAsync(url, clientId, _httpClient);

        //Assert
        response.Content.Should().BeNull();
        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

}
