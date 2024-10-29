using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.Client.ClientAccessToken;

internal static class GetClientTokenQueryBodyBuilder
{
    public static FormUrlEncodedContent ClientTokenRequestBody(GetClientTokenRequest client)
    {
        var data = new Dictionary<string, string>
        {
            {Constants.GrantType, Constants.GrantTypeCredentials},
            {Constants.ClientId, client.ClientId},
            {Constants.ClientSecret, client.ClientSecret }
        };
        return new FormUrlEncodedContent(data);
    }
}