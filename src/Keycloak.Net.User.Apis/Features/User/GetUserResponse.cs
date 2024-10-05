using Keycloak.Net.User.Apis.Common;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Keycloak.Net.User.Apis.Features.User;

internal record GetUserResponse
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