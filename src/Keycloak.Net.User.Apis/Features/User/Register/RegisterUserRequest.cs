using System.Text.Json.Serialization;

namespace Keycloak.Net.User.Apis.Features.User.Register;

internal record RegisterUserRequest()
{
    [JsonPropertyName("username")]
    public required string UserName { get; set; }
    [JsonPropertyName("email")]
    public required string Email { get; set; }
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; } = true;
    public string FirstName { get; set; } = string.Empty;
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; } = string.Empty;
    [JsonPropertyName("emailVerified")]
    public bool EmailVerified { get; set; } = true;
    [JsonPropertyName("credentials")]
    public required Credentials[] Credentials { get; set; }
}

internal record Credentials(string Value)
{
    [JsonPropertyName("type")]
    public string? Type { get; set; } = "password";
    [JsonPropertyName("value")]
    public string? Value { get; set; } = Value;
    [JsonPropertyName("temporary")]
    public bool Temporary { get; set; } = false;
}
