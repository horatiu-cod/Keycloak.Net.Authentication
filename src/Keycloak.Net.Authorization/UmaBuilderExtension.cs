using Keycloak.Net.Authorization.Configuration;
using Keycloak.Net.Authorization.PermissionAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Keycloak.Net.Authorization;

public static class UmaBuilderExtension
{
    public static IServiceCollection AddUma (this IServiceCollection services, Action<ClientConfiguration> options, Action<AuthorizationOptions>? configure = default)
    {
        services.AddUmaInternal();

        var message = $"Validation failed for {nameof(ClientConfiguration.ClientId)}";

        services.AddOptionsWithValidateOnStart<ClientConfiguration>().Validate(options =>
        {
            if (string.IsNullOrEmpty(options.ClientId))
            {
                return false;
            }
            return true;
        }, message).Configure(options);
        if (configure != null)
        {
            return services.AddAuthorization(configure);

        }
        return services.AddAuthorization();
    }
    public static IServiceCollection AddUma(this IServiceCollection services, string sectionName, Action<AuthorizationOptions>? configure = default)
    {
        services.AddUmaInternal();

        string message = $"Validation failed for {nameof(ClientConfiguration.ClientId)}";

        services.AddOptionsWithValidateOnStart<ClientConfiguration>().BindConfiguration(sectionName).Validate(options =>
        {
            if (string.IsNullOrEmpty(options.ClientId))
            {
                return false;
            }
            return true;
        }, message);
        if (configure != null)
        {
            return services.AddAuthorization(configure);

        }
        return services.AddAuthorization();
    }
    public static IServiceCollection AddUma(this IServiceCollection services, Action<AuthorizationOptions>? configure = default)
    {
        services.AddUmaInternal();

        if (configure != null)
        {
            return services.AddAuthorization(configure);

        }
        return services.AddAuthorization();
    }

    private static void AddUmaInternal(this IServiceCollection services)
    {
        services.AddHttpClient("uma");

        services.AddResiliencePipeline("default", x =>
        {
            x.AddRetry(new Polly.Retry.RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                Delay = TimeSpan.FromSeconds(2),
                MaxRetryAttempts = 2,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
            });
            x.AddTimeout(TimeSpan.FromSeconds(10));
        });


        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddScoped<IPermissionRequest, PermissionRequest>();
    }
}
