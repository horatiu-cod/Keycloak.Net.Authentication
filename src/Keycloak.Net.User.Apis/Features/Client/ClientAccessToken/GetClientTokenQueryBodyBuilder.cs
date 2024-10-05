using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;

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