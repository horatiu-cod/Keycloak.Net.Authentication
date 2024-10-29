using Keycloak.Net.User.Api.Common;
using System.Text.Json.Serialization;

namespace Keycloak.Net.User.Api.Features.User.Get;

public record GetUserResponse
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("username")]
    public string? UserName { get; set; }
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("emailVerified")]
    public bool EmailVerified { get; set; }
    [JsonPropertyName("createdTimestamp")]
    [JsonConverter(typeof(UnixEpochDateTimeConverter))]
    public DateTime CreatedTimestamp { get; set; }
}