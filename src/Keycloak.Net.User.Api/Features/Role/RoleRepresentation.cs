using System.Text.Json.Serialization;

namespace Keycloak.Net.User.Api.Features.Role;

internal record RoleRepresentation
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
