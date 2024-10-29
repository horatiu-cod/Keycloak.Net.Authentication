using System.Text.Json.Serialization;

namespace Keycloak.Net.User.Api.Features.User.RefreshToken;

internal record UserRefreshTokenRequest(string ClientId, string ClientSecret, string RefreshToken);
