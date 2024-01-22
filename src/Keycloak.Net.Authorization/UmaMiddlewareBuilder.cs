using Microsoft.AspNetCore.Builder;
using Keycloak.Net.Authorization.AudienceAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Keycloak.Net.Authorization.Configuration;

namespace Keycloak.Net.Authorization;

public static class UmaMiddlewareBuilder
{
    public static IApplicationBuilder UseUma(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UmaMiddleware>();
    }
}
internal class UmaMiddleware
{
    private readonly IOptions<ClientConfiguration> _options;
    private readonly RequestDelegate _next;

    public UmaMiddleware(IOptions<ClientConfiguration> options, RequestDelegate next)
    {
        _options = options;
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context, IAudienceAccessRequest _audienceAccessRequest)
    {
        if (context.Request.Headers.ContainsKey("Authorization") && !string.IsNullOrEmpty(context.Request.Headers.Authorization[0]) && context.Request.Headers.Authorization[0]!.StartsWith("Bearer"))
        {
            var token = context.Request.Headers.Authorization[0]!.Substring("Bearer ".Length);
            var rpt = await _audienceAccessRequest.VerifyRealmAccess(_options.Value.ClientId, token);
            if (rpt.IsSuccess)
            {
                context.Request.Headers.Remove("Authorization");
                var brt = $"Bearer {rpt.Content}";
                context.Request.Headers.Append("Authorization", brt);
            }
        }
        await _next(context);
    }
}
