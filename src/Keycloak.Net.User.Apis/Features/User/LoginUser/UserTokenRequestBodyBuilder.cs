using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User.LoginUser;

internal static class UserTokenRequestBodyBuilder
{
    public static FormUrlEncodedContent UserTokenRequestBody(UserTokenRequestDto userRequestDto)
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
