using FluentAssertions;
using Keycloak.Net.Authentication.Test.Integration.Abstraction;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.Authentication.Test.Integration.AuthenticationAndAuthorizationTests;
#pragma warning disable
public class RolesEndPoint : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;
    private readonly HttpClient _client;
    const string url = "https://localhost:8843/realms/oidc/protocol/openid-connect/token";
    const string apiUrl = "api/attribute";

    public RolesEndPoint(ApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
        _client = new HttpClient();
    }
    [Fact]
    public async void RolesEndPoint_WhenUserWithadminRole_ShouldReturnOk()
    {
        //Arrange
        await using var application = new WebApplicationFactory<Program>();

        using var testClient = application.CreateClient();
        var client = new HttpClient();
        var data = new Dictionary<string, string>();
        data.Add("grant_type", "password");
        data.Add("client_id", "public-client");
        data.Add("username", "hg@g.com");
        data.Add("password", "s3cr3t");

        var response = await client.PostAsync(url, new FormUrlEncodedContent(data));
        var content = await response.Content.ReadFromJsonAsync<JsonObject>();
        var token = content["access_token"].ToString();

        //Act
        testClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var result = await testClient.GetAsync(apiUrl);

        //Assert
        result.IsSuccessStatusCode.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
    [Fact]
    public async void RolesEndPoint_UserWithWrongRole_ShouldReturnForbidden()
    {
        //Arrange
        await using var application = new WebApplicationFactory<Program>();

        using var testClient = application.CreateClient();
        var client = new HttpClient();
        var data = new Dictionary<string, string>();
        data.Add("grant_type", "password");
        data.Add("client_id", "public-client");
        data.Add("username", "h@g.com");
        data.Add("password", "s3cr3t");

        var response = await client.PostAsync(url, new FormUrlEncodedContent(data));
        var content = await response.Content.ReadFromJsonAsync<JsonObject>();
        var token = content["access_token"].ToString();

        //Act
        testClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var result = await testClient.GetAsync(apiUrl);

        //Assert
        result.IsSuccessStatusCode.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }


}
