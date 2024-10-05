using FluentAssertions;
using Keycloak.Net.Authentication.JWT;
using System.Security.Claims;

namespace Keycloak.Net.Authentication.Test.Unit;
#pragma warning disable
public class TokenRoleMapperTest : IClassFixture<ClaimIdentityFixture>
{
    ClaimIdentityFixture _fixture;

    public TokenRoleMapperTest(ClaimIdentityFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void TokenRoleMapper_GetRoles_ShouldReturnListOfRoles()
    {
        //Arrange
        var claimsIdentity = (ClaimsIdentity?)_fixture.SetClaimsIdentity.Identity;

        //Act
        var roles = TokenRoleMapper.GetRoles(claimsIdentity, "audience");

        //Assert
        roles.Count().Should().Be(4);
        roles.Should().Contain("client_first_role");
        roles.Should().Contain("client_second_role");
        roles.Should().Contain("realm_first_role");
        roles.Should().Contain("realm_second_role");

    }
}
