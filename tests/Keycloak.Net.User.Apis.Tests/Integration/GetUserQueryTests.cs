using FluentAssertions;
using FluentAssertions.Execution;
using Keycloak.Net.User.Apis.Features.User.Get;
using Keycloak.Net.User.Apis.Tests.Integration.Abstraction;

namespace Keycloak.Net.User.Apis.Tests.Integration;

[Collection(nameof(KeycloakCollection))]
public class GetUserQueryTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)
{
    [Fact]
    public async Task Handler_ShouldReturnUserRepresentation_WhenUserExist()
    {
        //Assert
        var userName = "hg@g.com";
        var expectedUser = new GetUserResponse {UserName = userName, Id = "325ff607-8fe2-46bf-94fc-e0471a00ec70", FirstName = "hg", LastName ="g", Email = userName, EmailVerified = true,
            CreatedTimestamp = new DateTime(2024, 10, 09)
        };
        //Act
        var response = await _getUserQuery.Handler(userName);

        //Assert
        using(new AssertionScope()){
            response.Content.Should().BeEquivalentTo(expectedUser);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
    [Fact]
    public async Task Handler_ShouldReturnNotFound_WhenUserDoNotExist()
    {
        //Assert
        var userName = "a@g.com";

        //Act
        var response = await _getUserQuery.Handler(userName);

        using (new AssertionScope())
        {
            response.Content.Should().BeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
