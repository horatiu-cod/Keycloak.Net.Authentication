using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User.LoginUser;

internal static class GetUserTokenRequestBodyBuilder
{
    public static FormUrlEncodedContent UserTokenRequestBody(GetUserTokenRequest userRequestDto)
    {
        var data = new Dictionary<string, string>
        {
            {Constants.GrantType, Constants.GrantTypeCredentials},
            {Constants.ClientId, userRequestDto.ClientId},
            {Constants.ClientSecret, userRequestDto.ClientSecret },
            {Constants.Username, userRequestDto.UserName },
            {Constants.Password, userRequestDto.Password},
        };
        return new FormUrlEncodedContent(data);
    }
}
