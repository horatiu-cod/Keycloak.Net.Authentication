using System.Text.Json.Serialization;

namespace Keycloak.Net.User.Apis.Features.User.DeleteUser;

internal record DeleteUserRequest
{
    [JsonPropertyName("id")]
    public string? UserId { get; init; }
}
