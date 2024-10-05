using DotNet.Testcontainers.Builders;
using Testcontainers.Keycloak;

namespace Keycloak.Net.User.Apis.Tests.Integration;

public class KeycloakFixture : IAsyncLifetime
{
    private readonly KeycloakContainer _container = new KeycloakBuilder()
        .WithImage("quay.io/keycloak/keycloak:25.0")
        .WithName("keycloak2")
        .WithPortBinding(8845, 8443)
        .WithResourceMapping("./Integration/realm-import.json", "/opt/keycloak/data/import")
        //.WithCommand("--import-realm")
        .WithResourceMapping("./Integration/localhostcert.pem", @"/opt/keycloak/certs")
        .WithResourceMapping("./Integration/localhostkey.pem", @"/opt/keycloak/certs")
        .WithEnvironment(@"KC_HTTPS_CERTIFICATE_FILE", @"/opt/keycloak/certs/localhostcert.pem")
        .WithEnvironment(@"KC_HTTPS_CERTIFICATE_KEY_FILE", @"/opt/keycloak/certs/localhostkey.pem")
        .WithCommand( "--import-realm" )

        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8443))
        .WithStartupCallback((c, CancellationToken )=> c.ExecAsync(["--import-realm"]))
        .Build();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
    }
}