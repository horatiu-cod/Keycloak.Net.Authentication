using System.Text.Json.Serialization;

namespace Keycloak.Net.User.Apis.Features.Role;

internal record struct RoleDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
