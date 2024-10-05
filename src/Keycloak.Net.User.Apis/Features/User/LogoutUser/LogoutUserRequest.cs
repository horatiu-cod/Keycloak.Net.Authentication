namespace Keycloak.Net.User.Apis.Features.User.LogoutUser;

internal record LogoutUserRequest(string ClientId, string ClientSecret, string RefreshToken);
