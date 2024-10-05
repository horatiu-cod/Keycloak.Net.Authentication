using System.Text.Json.Serialization;

namespace Keycloak.Net.User.Apis.Features.User.UserRefreshToken;

internal record UserRefreshTokenRequest(string ClientId, string ClientSecret, string RefreshToken);
