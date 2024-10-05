using FluentAssertions;
using Keycloak.Net.Authentication.Test.Integration.Abstraction;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.Authentication.Test.Integration.AuthenticationAndAuthorizationTests;
#pragma warning disable
public class UmaEndPoints : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;
    private readonly HttpClient _client;
    const string url = "https://localhost:8843/realms/oidc/protocol/openid-connect/token";

    public UmaEndPoints(ApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
        _client = new HttpClient();
    }

    [Theory]
    [InlineData("api/uma1")]
    [InlineData("api/uma2")]
    [InlineData("api/uma3")]
    public async void UmaEndPointsTest_WithAuthorizedUserWithRole_ReturnOk(string apiUrl)
    {
        //Arrange
        var data = new Dictionary<string, string>();
        data.Add("grant_type", "password");
        data.Add("client_id", "public-client");
        data.Add("username", "hg@g.com");
        data.Add("password", "s3cr3t");

        //var apiUrl = "api/attribute";

        var response = await _client.PostAsync(url, new FormUrlEncodedContent(data));
        var content = await response.Content.ReadFromJsonAsync<JsonObject>();
        var token = content["access_token"].ToString();

        //Act
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var result = await _httpClient.GetAsync(apiUrl);

        //Assert
        result.IsSuccessStatusCode.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
    [Theory]
    [InlineData("api/uma1")]
    [InlineData("api/uma2")]
    [InlineData("api/uma3")]
    public async void UmaEndPointsTest_WithoutAuthorizedUserWithoutRole_ReturnUnauthorizedOrForbidden(string apiUrl)
    {
        //Arrange
        var data = new Dictionary<string, string>();
        data.Add("grant_type", "password");
        data.Add("client_id", "public-client");
        data.Add("username", "h@g.com");
        data.Add("password", "s3cr3t");

        var response = await _client.PostAsync(url, new FormUrlEncodedContent(data));
        var content = await response.Content.ReadFromJsonAsync<JsonObject>();
        var token = content["access_token"].ToString();

        //Act
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var result = await _httpClient.GetAsync(apiUrl);

        //Assert
        result.IsSuccessStatusCode.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

}
