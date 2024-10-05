using System.Text.Json.Serialization;

namespace Keycloak.Net.User.Apis.Features.Role;

internal record RoleRequest
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

}
