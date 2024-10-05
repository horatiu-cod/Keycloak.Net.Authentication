using System.Text.Json.Serialization;

namespace Keycloak.Net.User.Apis.Features.User.UpdateUser;

internal record UpdateUserRequest
{
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
}
