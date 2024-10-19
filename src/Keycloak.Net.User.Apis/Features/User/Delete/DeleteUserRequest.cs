using System.Text.Json.Serialization;

namespace Keycloak.Net.User.Apis.Features.User.Delete;

internal record DeleteUserRequest
{
    [JsonPropertyName("id")]
    public string? UserId { get; init; }
}
