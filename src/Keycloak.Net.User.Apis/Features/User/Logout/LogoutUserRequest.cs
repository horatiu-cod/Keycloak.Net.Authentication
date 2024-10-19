namespace Keycloak.Net.User.Apis.Features.User.Logout;

internal record LogoutUserRequest(string ClientId, string ClientSecret, string RefreshToken);
