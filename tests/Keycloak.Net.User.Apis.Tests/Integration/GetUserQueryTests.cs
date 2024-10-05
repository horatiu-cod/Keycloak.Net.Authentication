using FluentAssertions;
using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.User;

namespace Keycloak.Net.User.Apis.Tests.Integration;

public class GetUserQueryTests : IClassFixture<TokenRequestFixture>
{
    readonly HttpClient _httpClient;
    readonly string url = BaseUrl.AdminUrl("https://localhost:8843/", "oidc");
    private readonly IGetUserQuery _sut;

    public GetUserQueryTests(TokenRequestFixture tokenRequest)
    {
        _httpClient = tokenRequest._httpClient; 
        _sut = new GetUserQuery();
    }
    [Fact]
    public async Task GetUserAsync_ShouldReturnUserRepresentation_WhenUserExist()
    {
        //Assert
        var userName = "hg@g.com";
        var expectedUser = new GetUserResponse {UserName = userName, Id = "9105214e-846e-43a1-a6b8-902ed5c74305" , FirstName = "Hori", LastName ="Cod", Email = userName, EmailVerified = true,
            CreatedTimestamp = new DateTime(2024, 01, 17)
        };
        //Act
        var response = await _sut.GetUserAsync(url, userName, _httpClient);

        //Assert
        response.Content.Should().BeEquivalentTo(expectedUser);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
    [Fact]
    public async Task GetUserAsync_ShouldReturnUserId_WhenUserExist()
    {
        //Assert
        var userName = "hg@g.com";
        var expectedUserId = "9105214e-846e-43a1-a6b8-902ed5c74305";

        //Act
        var response = await _sut.GetUserIdAsync(url, userName, _httpClient);

        //Assert
        response.Content.Should().Be(expectedUserId);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetUserAsync_ShouldReturnNotFound_WhenUserDoNotExist()
    {
        //Assert
        var userName = "hgg@g.com";

        //Act
        var response = await _sut.GetUserIdAsync(url, userName, _httpClient);

        //Assert
        response.Content.Should().BeNullOrEmpty();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
