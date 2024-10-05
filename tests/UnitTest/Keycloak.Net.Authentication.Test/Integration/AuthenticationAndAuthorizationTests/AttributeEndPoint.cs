using FluentAssertions;
using Keycloak.Net.Authentication.Test.Integration.Abstraction;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.Authentication.Test.Integration.AuthenticationAndAuthorizationTests;
#pragma warning disable
public class AttributeEndPoint : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;
    private readonly HttpClient _client;

    public AttributeEndPoint(ApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
        _client = new HttpClient();
    }

    [Fact]
    public async void AttributeEndPoint_UserWithRole_ShouldReturnOk()
    {
        //Arrange
        var data = new Dictionary<string, string>();
        data.Add("grant_type", "password");
        data.Add("client_id", "public-client");
        data.Add("username", "hg@g.com");
        data.Add("password", "s3cr3t");

        var url = "https://localhost:8843/realms/oidc/protocol/openid-connect/token";
        var apiUrl = "api/attribute";

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
    [Fact]
    public async void AttributeEndPoint_WhenUserIsWithoutRole_ShouldReturnUnauthorizedOrForbidden()
    {
        //Arrange
        var data = new Dictionary<string, string>();
        data.Add("grant_type", "password");
        data.Add("client_id", "public-client");
        data.Add("username", "h@g.com");
        data.Add("password", "s3cr3t");

        var url = "https://localhost:8843/realms/oidc/protocol/openid-connect/token";
        var apiUrl = "api/attribute";

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
