using Microsoft.AspNetCore.Builder;

namespace Keycloak.Net.Authorization;

public static class AuthorizationEndpointConventionBuilderExtensions
{
    /// <summary>
    /// Adds the custom authorization policy to the endpoint(s).
    /// </summary>
    /// <param name="builder">The endpoint convention builder.</param>
    /// <returns>The original convention builder parameter.</returns>
    public static TBuilder RequireUmaAuthorization<TBuilder>(this TBuilder builder, string resource, string scope) where TBuilder : IEndpointConventionBuilder
    {
        if (object.Equals(builder, default(TBuilder)))
        {
            throw new ArgumentNullException(nameof(builder));
        }

        return builder.RequireAuthorization(new PermissionAttribute(resource, scope));
    }

}
