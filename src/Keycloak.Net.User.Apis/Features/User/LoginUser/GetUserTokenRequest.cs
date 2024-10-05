namespace Keycloak.Net.User.Apis.Features.User.LoginUser;

internal record GetUserTokenRequest(string ClientId, string ClientSecret, string UserName, string Password);
