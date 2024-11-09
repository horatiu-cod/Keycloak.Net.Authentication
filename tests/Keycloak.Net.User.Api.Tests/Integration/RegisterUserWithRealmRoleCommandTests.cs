﻿using FluentAssertions.Execution;
using FluentAssertions;
using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Tests.Integration.Abstraction;

namespace Keycloak.Net.User.Api.Tests.Integration;

public class RegisterUserWithRealmRoleCommandTests(KeycloakFixture keycloakFixture) : BaseIntegrationTest(keycloakFixture)
{
    [Fact]
    public async Task Handler_ShouldReturnCreated_WhenUserRegisteredSuccessfully()
    {
        //Arrange
        var username = "zc@g.com";
        var password = "password";
        string email = "zc@g.com";
        string firstName = "z";
        string lastName = "g";

        var url = BaseUrl.AdminUrl(_baseAddress, _realmName);
        var clientId = "frontend";
        var roleName = "realm-test";


        //Act
        var response = await _registerUserWithRealmRoleCommand.Handler(username, password, email, firstName, lastName, [roleName]);
        var user = await _getUserIdQuery.GetUserIdAsync(url, username, _httpClient);
        var userToken = await _getUserTokenQuery.LoginUserAsync(clientId, username, password);
        //assert
        using (new AssertionScope())
        {
            response.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            user.IsSuccess.Should().BeTrue();
            userToken.IsSuccess.Should().BeTrue();
        }
    }
    [Fact]
    public async Task Handler_ShouldReturnConflict_WhenUserAlreadyRegistered()
    {
        //Arrange
        var username = "hg@g.com";
        var password = "passwords";
        string email = "hg@g.com";
        string firstName = "c";
        string lastName = "z";

        var roleName = "realm-test";

        //Act
        var response = await _registerUserWithRealmRoleCommand.Handler(username, password, email, firstName, lastName, [roleName]);

        //assert
        using (new AssertionScope())
        {
            response.IsSuccess.Should().BeFalse();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
        }
    }
}