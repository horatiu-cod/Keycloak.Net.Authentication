using DotNet.Testcontainers.Builders;
using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Features.Client.ClientAccessToken;


//using Keycloak.Net.User.Api.Common;
//using Keycloak.Net.User.Api.Features.Client.ClientAccessToken;
using System.Net.Http.Headers;
using Testcontainers.Keycloak;

namespace Keycloak.Net.User.Api.Tests.Integration.Abstraction;

public class KeycloakFixture : IAsyncLifetime
{
    public HttpClient HttpClient { get; set; } = new HttpClient();
    public readonly string clientId = "management";
    public readonly string clientSecret = "2bpVgqGkUwUuagkJZ1DLK5Ncb3fkO1ri";
    public readonly string realmName = "oidc";
    public string BaseAddress { get; private set; } = "https://localhost:8843";

    private readonly KeycloakContainer _container = new KeycloakBuilder()
        .WithImage("keycloak/keycloak:24.0")
        //.WithExposedPort(8443)
        .WithPortBinding(8843,8443)
        .WithEnvironment("KC_HTTP_ENABLED", "false")
        .WithResourceMapping("./Integration/Import/oidc.json", "/opt/keycloak/data/import")
        .WithResourceMapping("./Integration/Certs", @"/opt/keycloak/certs")
        .WithCommand("--import-realm")
        .WithEnvironment(@"KC_HTTPS_CERTIFICATE_FILE", @"/opt/keycloak/certs/cert.pem")
        .WithEnvironment(@"KC_HTTPS_CERTIFICATE_KEY_FILE", @"/opt/keycloak/certs/key.key")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
        .Build();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        //BaseAddress = _container.GetBaseAddress();
        HttpClient = await GetTokenAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
    }

    public async Task<HttpClient> GetTokenAsync()
    {
        var _clientTokenRequest = new GetClientTokenQuery();
        ArgumentException.ThrowIfNullOrEmpty(nameof(BaseAddress));
        var client = new GetClientTokenRequest(clientId, clientSecret);
        var response = await _clientTokenRequest.GetClientTokenAsync(BaseUrl.TokenUrl(BaseAddress!, realmName), client, HttpClient);
        if (response.IsSuccess)
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Content?.AccessToken);

        }
        return HttpClient;
    }
}