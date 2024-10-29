using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.User.Logout;

internal static class LogoutUserRequestBodyBuilder
{
    internal static FormUrlEncodedContent LogoutUserTokenRequestBody(LogoutUserRequest logoutUserRequestDto)
    {
        var data = new Dictionary<string, string>
        {
            {Constants.ClientId, logoutUserRequestDto.ClientId},
            {Constants.ClientSecret, logoutUserRequestDto.ClientSecret},
            {Constants.RefreshToken,  logoutUserRequestDto.RefreshToken},
        };
        return new FormUrlEncodedContent(data);
    }
}
