namespace Keycloak.Net.User.Apis.Features.User.Login;

internal record GetUserTokenRequest(string ClientId, string ClientSecret, string UserName, string Password);
