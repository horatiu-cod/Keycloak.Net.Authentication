using FluentAssertions;
using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.User;

namespace Keycloak.Net.User.Apis.Tests.Integration;

//[Collection(nameof(KeycloakCollection))]
public class GetUserQueryTests(KeycloakFixture keycloakFixture) : IClassFixture<KeycloakFixture>
{
    readonly string url = BaseUrl.AdminUrl(keycloakFixture.BaseAddress?? string.Empty, "oidc");
    readonly HttpClient httpClient = keycloakFixture.HttpClient;
    private readonly IGetUserQuery _sut = new GetUserQuery();

    [Fact]
    public async Task GetUserAsync_ShouldReturnUserRepresentation_WhenUserExist()
    {
        //Assert
        var userName = "hg@g.com";
        var expectedUser = new GetUserResponse {UserName = userName, Id = "325ff607-8fe2-46bf-94fc-e0471a00ec70", FirstName = "hg", LastName ="g", Email = userName, EmailVerified = true,
            CreatedTimestamp = new DateTime(2024, 10, 09)
        };
        //Act
        var response = await _sut.GetUserAsync(url, userName, httpClient);

        //Assert
        response.Content.Should().BeEquivalentTo(expectedUser);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
    [Fact]
    public async Task GetUserAsync_ShouldReturnUserId_WhenUserExist()
    {
        //Assert
        var userName = "hg@g.com";
        var expectedUserId = "325ff607-8fe2-46bf-94fc-e0471a00ec70";

        //Act
        var response = await _sut.GetUserIdAsync(url, userName, httpClient);

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
        var response = await _sut.GetUserIdAsync(url, userName, httpClient);

        //Assert
        response.Content.Should().BeNullOrEmpty();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
