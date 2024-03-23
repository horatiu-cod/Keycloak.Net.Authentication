using System.Text.Json.Serialization;

namespace Keycloak.Net.User.Apis.Features.User.UserRefreshToken;

internal class UserRefreshTokenRequestDto
{
    [JsonPropertyName("grant_type")]
    public string? GrantType { get; set; }
    [JsonPropertyName("client_id")]
    public string? ClientId { get; set; }
    [JsonPropertyName("client_secret")]
    public string? ClientSecret { get; set; }
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

}
