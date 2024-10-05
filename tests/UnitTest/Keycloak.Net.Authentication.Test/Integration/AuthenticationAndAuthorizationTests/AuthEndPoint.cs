using FluentAssertions;
using Keycloak.Net.Authentication.Test.Integration.Abstraction;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.Authentication.Test.Integration.AuthenticationAndAuthorizationTests;
#pragma warning disable
public class AuthEndPoint : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;
    private readonly HttpClient _client;
    private const string url = "https://localhost:8843/realms/oidc/protocol/openid-connect/token";
    private const string apiUrl = "api/auth";

    public AuthEndPoint(ApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
        _client = new HttpClient();
    }
    [Fact]
    public async void AuthEndPoint_WhenUserIsAuthorized_ShouldReturnOk()
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
        result.IsSuccessStatusCode.Should().BeTrue();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
    //[Fact]
    //public async void AuthEndPointTest_MultipleRequestWithAuthorizeUserWithRole_ReturnOk()
    //{
    //    //Arrange
    //    await using var application = new WebApplicationFactory<Program>();

    //    using var testClient = application.CreateClient();
    //    var client = new HttpClient();
    //    var data = new Dictionary<string, string>();
    //    data.Add("grant_type", "password");
    //    data.Add("client_id", "client-role");
    //    data.Add("client_secret", "Jj4hCpRdezqvSVDjxXmNRWZ5CTRsg14p");
    //    data.Add("username", "h@g.com");
    //    data.Add("password", "s3cr3t");

    //    var url = "https://localhost:8843/realms/uma/protocol/openid-connect/token";
    //    var apiUrl = "api/auth";

    //    var response = await client.PostAsync(url, new FormUrlEncodedContent(data));
    //    var content = await response.Content.ReadFromJsonAsync<JsonObject>();
    //    var token = content["access_token"].ToString();


    //    //Act
    //    for (int i = 0; i < 10; i++)
    //    {
    //        testClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    //        var result = await testClient.GetAsync(apiUrl);

    //        //Assert
    //        result.IsSuccessStatusCode.Should().BeTrue();
    //        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    //    }
    //}
    [Fact]
    public async void AuthEndPoint_WhenUserIsNotAuthorized_ShouldReturnUnauthorizedOrForbidden()
    {
        //Act
        //_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var result = await _httpClient.GetAsync(apiUrl);

        //Assert
        result.IsSuccessStatusCode.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

}
