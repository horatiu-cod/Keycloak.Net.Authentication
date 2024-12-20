﻿using FluentAssertions;
using Keycloak.Net.Authentication.Extensions;
using Keycloak.Net.Authentication.JWT;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Keycloak.Net.Authentication.Test.Unit;
#pragma warning disable
public class KeycloakClaimsTransformationTest
{
    public IOptions<JwtBearerValidationOptions> _jwtBearerValidationOptions { get; }

    public KeycloakClaimsTransformationTest()
    {
        _jwtBearerValidationOptions = Options.Create(new JwtBearerValidationOptions());
    }

    [Fact]
    public async Task KeycloakClaimsTransformation_TransformAsync_ShouldReturnJwtIdentityType()
    {
        //Arrange
        ClaimIdentityFixture _fixture = new ClaimIdentityFixture();

        _jwtBearerValidationOptions.Value.Authority = "https://keycloak.mydomain.com/realms/realm";
        _jwtBearerValidationOptions.Value.Audience = "audience";
        var transformation = new KeycloakClaimsTransformation(_jwtBearerValidationOptions);
        var claimsPrincipal = _fixture.SetClaimsIdentity;

        //Act
        await transformation.TransformAsync(claimsPrincipal);
        var claimsIdentity = (ClaimsIdentity?)_fixture.SetClaimsIdentity.Identity;
        //Assert
        claimsIdentity.TryGetClaim(c => c.Type == ClaimTypes.Name, out var claim).Should().BeTrue();
        claim.Value.Should().Be("horatiu");
        claimsIdentity.HasClaim(ClaimTypes.Role, "client_first_role").Should().BeTrue();
        claimsIdentity.HasClaim(ClaimTypes.Role, "client_second_role").Should().BeTrue();
        claimsIdentity.HasClaim(ClaimTypes.Role, "realm_first_role").Should().BeTrue();
        claimsIdentity.HasClaim(ClaimTypes.Role, "realm_second_role").Should().BeTrue();
        claimsIdentity.Claims.Count(item => ClaimTypes.Role == item.Type).Should().Be(4);
    }
    [Fact]
    public async Task KeycloakClaimsTransformation_TransformAsync_ShouldNotTransformIfIssuerIsNotSetCorrectly()
    {
        //Arrange
        ClaimIdentityFixture _fixture = new ClaimIdentityFixture();

        _jwtBearerValidationOptions.Value.Authority = "https://keycloak.mydomain.com/realms/wrong_realm";
        _jwtBearerValidationOptions.Value.Audience = "audience";
        var transformation = new KeycloakClaimsTransformation(_jwtBearerValidationOptions);
        var claimsPrincipal = _fixture.SetClaimsIdentity;

        //Act
        await transformation.TransformAsync(claimsPrincipal);
        var claimsIdentity = (ClaimsIdentity?)_fixture.SetClaimsIdentity.Identity;
        //Assert
        claimsIdentity.TryGetClaim(c => c.Type == ClaimTypes.Name, out var claim).Should().BeTrue();
        claim.Value.Should().Be("horatiu cod");
        claimsIdentity.HasClaim(ClaimTypes.Role, "client_first_role").Should().BeFalse();
        claimsIdentity.HasClaim(ClaimTypes.Role, "client_second_role").Should().BeFalse();
        claimsIdentity.HasClaim(ClaimTypes.Role, "realm_first_role").Should().BeFalse();
        claimsIdentity.HasClaim(ClaimTypes.Role, "realm_second_role").Should().BeFalse();
        claimsIdentity.Claims.Count(item => ClaimTypes.Role == item.Type).Should().Be(0);
    }
    [Fact]
    public async Task KeycloakClaimsTransformation_TransformAsync_ShouldNotTransformOnlyClientRolesIfAudienceIsNull()
    {
        //Arrange
        ClaimIdentityFixture _fixture = new ClaimIdentityFixture();
        _jwtBearerValidationOptions.Value.Authority = "https://keycloak.mydomain.com/realms/realm";
        _jwtBearerValidationOptions.Value.Audience = "";
        var transformation = new KeycloakClaimsTransformation(_jwtBearerValidationOptions);
        var claimsPrincipal = _fixture.SetClaimsIdentity;

        //Act
        await transformation.TransformAsync(claimsPrincipal);
        var claimsIdentity = (ClaimsIdentity?)_fixture.SetClaimsIdentity.Identity;
        //Assert
        claimsIdentity.TryGetClaim(c => c.Type == ClaimTypes.Name, out var claim).Should().BeTrue();
        claim.Value.Should().Be("horatiu");
        claimsIdentity.HasClaim(ClaimTypes.Role, "client_first_role").Should().BeFalse();
        claimsIdentity.HasClaim(ClaimTypes.Role, "client_second_role").Should().BeFalse();
        claimsIdentity.HasClaim(ClaimTypes.Role, "realm_first_role").Should().BeTrue();
        claimsIdentity.HasClaim(ClaimTypes.Role, "realm_second_role").Should().BeTrue();
        claimsIdentity.Claims.Count(item => ClaimTypes.Role == item.Type).Should().Be(2);
    }
}
