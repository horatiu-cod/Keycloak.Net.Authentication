using DotNet.Testcontainers.Builders;
using Keycloak.Net.Authentication.MinimalApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.Keycloak;

namespace Keycloak.Net.Authentication.Test.Integration.Abstraction;

public class ApiFactory : WebApplicationFactory<IApiMarker> , IAsyncLifetime
{
    public string? BaseAddress { get; set; } = "https://localhost:8843";

    private readonly KeycloakContainer _container = new KeycloakBuilder()
        .WithImage("keycloak/keycloak:24.0")
        .WithReuse(true)
        .WithPortBinding(8843, 8443)
        .WithResourceMapping("./Integration/import/oidc.json", "/opt/keycloak/data/import")
        .WithResourceMapping("./Integration/Certs/localhostcert.pem", @"/opt/keycloak/certs")
        .WithResourceMapping("./Integration/Certs/localhostkey.pem", @"/opt/keycloak/certs")
        .WithReuse(true)
        .WithEnvironment(@"KC_HTTPS_CERTIFICATE_FILE", @"/opt/keycloak/certs/localhostcert.pem")
        .WithEnvironment(@"KC_HTTPS_CERTIFICATE_KEY_FILE", @"/opt/keycloak/certs/localhostkey.pem")
        .WithCommand("--import-realm")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8443))
        .Build();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _container.StopAsync();
    }
}
