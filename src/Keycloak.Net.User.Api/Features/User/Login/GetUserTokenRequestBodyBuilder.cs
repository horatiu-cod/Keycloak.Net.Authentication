﻿using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.User.Login;

internal static class GetUserTokenRequestBodyBuilder
{
    public static FormUrlEncodedContent UserTokenRequestBody(GetUserTokenRequest userRequestDto)
    {
        var data = new Dictionary<string, string>
        {
            {Constants.GrantType, Constants.GrantTypePassword},
            {Constants.ClientId, userRequestDto.ClientId},
            {Constants.ClientSecret, userRequestDto.ClientSecret },
            {Constants.Username, userRequestDto.UserName },
            {Constants.Password, userRequestDto.Password},
        };
        return new FormUrlEncodedContent(data);
    }
}