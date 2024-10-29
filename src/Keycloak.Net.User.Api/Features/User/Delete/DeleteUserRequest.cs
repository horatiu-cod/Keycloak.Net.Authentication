using System.Text.Json.Serialization;

namespace Keycloak.Net.User.Api.Features.User.Delete;

internal record DeleteUserRequest
{
    [JsonPropertyName("id")]
    public string? UserId { get; init; }
}
