using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User.RefreshToken;

internal static class UserRefreshTokenRequestBodyBuilder
{
    internal static FormUrlEncodedContent UserRefreshTokenRequestBody(UserRefreshTokenRequest userRefreshTokenRequest)
    {
        var data = new Dictionary<string, string>
        {
            {Constants.GrantType, Constants.GrantTypeRefreshToken},
            {Constants.ClientId, userRefreshTokenRequest.ClientId},
            {Constants.ClientSecret, userRefreshTokenRequest.ClientSecret},
            {Constants.RefreshToken, userRefreshTokenRequest.RefreshToken},
        };
        return new FormUrlEncodedContent(data);
    }
}
