﻿namespace Keycloak.Net.User.Api.Features.User.Login;

internal record GetUserTokenRequest(string ClientId, string ClientSecret, string UserName, string Password);
