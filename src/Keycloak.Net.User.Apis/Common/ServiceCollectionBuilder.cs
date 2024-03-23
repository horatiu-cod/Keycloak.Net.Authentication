using Microsoft.Extensions.DependencyInjection;

namespace Keycloak.Net.User.Apis.Common;

public static class ServiceCollectionBuilder
{
    public static IServiceCollection AddKeycloakApi(this IServiceCollection services)
    {
        services.AddHttpClient("api");
        services.AddSingleton<IRequestUrlBuilder, RequestUrlBuilder>();
        return services;
    }
}
