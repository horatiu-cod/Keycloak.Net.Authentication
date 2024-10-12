using Microsoft.AspNetCore.Builder;

namespace Keycloak.Net.Authorization;

public static class EndPointWhitClientMetadataBuilder
{
    public static TBuilder ResourceClient<TBuilder>(this TBuilder builder, string clientName) where TBuilder : IEndpointConventionBuilder
    {
        builder.Add(endpointBuilder =>
        {
            endpointBuilder.Metadata.Add(new ClientNameAttribute(clientName));
        });
        return builder;
    }
}
