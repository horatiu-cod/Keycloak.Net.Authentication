using DotNet.Testcontainers.Builders;
using Keycloak.Net.Authentication.MinimalApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.Keycloak;

namespace Keycloak.Net.Authentication.Test.Integration.Abstraction;

public class ApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    public string? BaseAddress { get; set; } = "http://localhost:8181";


    private readonly KeycloakContainer _container = new KeycloakBuilder()
        .WithImage("keycloak/keycloak:24.0")
        .WithPortBinding(8181, 8080)
        //.WithPortBinding(8843, 8443)
        .WithResourceMapping("./Integration/import/oidc.json", "/opt/keycloak/data/import")
        //.WithResourceMapping("./Integration/Certs/localhostcert.pem", @"/opt/keycloak/certs")
        //.WithResourceMapping("./Integration/Certs/localhostkey.pem", @"/opt/keycloak/certs")
        //.WithEnvironment(@"KC_HTTPS_CERTIFICATE_FILE", @"/opt/keycloak/certs/localhostcert.pem")
        //.WithEnvironment(@"KC_HTTPS_CERTIFICATE_KEY_FILE", @"/opt/keycloak/certs/localhostkey.pem")
        .WithCommand("--import-realm")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddOptions<JwtBearerValidationOptions>().Configure(options =>
            {
                options.RequireHttpsMetadata = false;
            });

        });
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _container.StopAsync();
    }
}
