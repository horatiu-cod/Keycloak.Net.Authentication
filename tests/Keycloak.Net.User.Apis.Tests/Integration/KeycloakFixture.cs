﻿using DotNet.Testcontainers.Builders;
using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;
using System.Net.Http.Headers;
using Testcontainers.Keycloak;

namespace Keycloak.Net.User.Apis.Tests.Integration;

public class KeycloakFixture : IAsyncLifetime
{
    public HttpClient HttpClient { get; set; } = new HttpClient();
    const string clientId = "management";
    const string clientSecret = "2bpVgqGkUwUuagkJZ1DLK5Ncb3fkO1ri";
    readonly string realmName = "oidc";
    public string? BaseAddress { get; set; }

    private readonly KeycloakContainer _container = new KeycloakBuilder()
        .WithImage("keycloak/keycloak:24.0")
        .WithExposedPort(8443)
        .WithEnvironment("KC_HTTP_ENABLED", "false")
        .WithResourceMapping("./Integration/oidc.json", "/opt/keycloak/data/import")
        .WithCommand("--import-realm")
        //.WithResourceMapping("./Integration/localhostcert.pem", @"/opt/keycloak/certs")
        //.WithResourceMapping("./Integration/localhostkey.pem", @"/opt/keycloak/certs")
        //.WithReuse(true)
        //.WithEnvironment(@"KC_HTTPS_CERTIFICATE_FILE", @"/opt/keycloak/certs/localhostcert.pem")
        //.WithEnvironment(@"KC_HTTPS_CERTIFICATE_KEY_FILE", @"/opt/keycloak/certs/localhostkey.pem")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
        .Build();

    public async  Task InitializeAsync()
    {
        await _container.StartAsync();
        BaseAddress = _container.GetBaseAddress();
        HttpClient = await GetTokenAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
    }

    public async Task<HttpClient> GetTokenAsync()
    {
        var _clientTokenRequest = new GetClientTokenQuery();
        ArgumentNullException.ThrowIfNullOrEmpty(nameof(BaseAddress));
        var client = new GetClientTokenRequest(clientId, clientSecret);
        var response = await _clientTokenRequest.GetClientTokenAsync(BaseUrl.TokenUrl(BaseAddress!, realmName), client, HttpClient);
        if (response.IsSuccess)
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Content?.AccessToken);

        }
        return HttpClient;
    }

}