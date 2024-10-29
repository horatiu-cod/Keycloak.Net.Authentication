namespace Keycloak.Net.User.Api.Features.User.Logout;

internal record LogoutUserRequest(string ClientId, string ClientSecret, string RefreshToken);
