using FluentAssertions;
using Keycloak.Net.Authentication.Test.Integration.Abstraction;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.Authentication.Test.Integration.AuthenticationAndAuthorizationTests;

[Collection(nameof(ApiFactoryCollection))]
public class UmaEndPointsTests(ApiFactory apiFactory)
{
    private readonly HttpClient _httpClient = apiFactory.CreateClient();
    private readonly HttpClient _client = new();
    private readonly string? _baseAddress = apiFactory.BaseAddress;

    [Theory]
    [InlineData("api/uma1")]
    [InlineData("api/uma2")]
    [InlineData("api/uma3")]
    public async Task UmaEndPointsTest_WithAuthorizedUserWithRole_ReturnOk(string apiUrl)
    {
        //Arrange
        string url = $"{_baseAddress}/realms/oidc/protocol/openid-connect/token";
        var data = new Dictionary<string, string>();
        data.Add("grant_type", "password");
        data.Add("client_id", "frontend");
        data.Add("username", "h@g.com");
        data.Add("password", "s3cr3t");

        var response = await _client.PostAsync(url, new FormUrlEncodedContent(data));
        var content = await response.Content.ReadFromJsonAsync<JsonObject>();
        var token = content?["access_token"]?.ToString();

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
    public async Task UmaEndPointsTest_WithoutAuthorizedUserWithoutRole_ReturnUnauthorizedOrForbidden(string apiUrl)
    {
        //Arrange
        string url = $"{_baseAddress}/realms/oidc/protocol/openid-connect/token";

        var data = new Dictionary<string, string>();
        data.Add("grant_type", "password");
        data.Add("client_id", "frontend");
        data.Add("username", "hg@g.com");
        data.Add("password", "s3cr3t");

        var response = await _client.PostAsync(url, new FormUrlEncodedContent(data));
        var content = await response.Content.ReadFromJsonAsync<JsonObject>();
        var token = content?["access_token"]?.ToString();

        //Act
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var result = await _httpClient.GetAsync(apiUrl);

        //Assert
        result.IsSuccessStatusCode.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }
}
