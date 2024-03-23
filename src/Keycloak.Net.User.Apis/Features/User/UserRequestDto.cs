using System.Text.Json.Serialization;

namespace Keycloak.Net.User.Apis.Features.User;

internal record struct UserRequestDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("username")]
    public string? UserName { get; set; }
}
