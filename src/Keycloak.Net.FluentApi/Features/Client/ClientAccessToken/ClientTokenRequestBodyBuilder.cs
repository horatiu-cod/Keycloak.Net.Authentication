using Keycloak.Net.FluentApi.Common;

namespace Keycloak.Net.FluentApi.Features.Client.ClientAccessToken;

internal static class ClientTokenRequestBodyBuilder
{
    public static FormUrlEncodedContent ClientTokenRequestBody(ClientTokenRequestDto clientDto)
    {
        var data = new Dictionary<string, string>
        {
            {Constants.GrantType, Constants.GrantTypeCredentials},
            {Constants.ClientId, clientDto.ClientId},
            {Constants.ClientSecret, clientDto.ClientSecret }
        };
        return new FormUrlEncodedContent(data);
    }
}
