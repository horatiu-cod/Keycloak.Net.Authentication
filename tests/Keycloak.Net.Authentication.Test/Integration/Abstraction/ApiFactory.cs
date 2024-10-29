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
        .WithResourceMapping("./Integration/import/oidc.json", "/opt/keycloak/data/import")
        .WithCommand("--import-realm")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddOptions<JwtBearerValidationOptions>().Configure(options =>
            {
                options.Authority = "http://localhost:8181/realms/oidc";
                options.Audience = "frontend";
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
