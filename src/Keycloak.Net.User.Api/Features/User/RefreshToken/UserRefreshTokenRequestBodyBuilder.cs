using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.User.RefreshToken;

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
