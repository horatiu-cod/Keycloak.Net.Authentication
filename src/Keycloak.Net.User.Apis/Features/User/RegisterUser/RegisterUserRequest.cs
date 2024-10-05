using System.Text.Json.Serialization;
using System.Text.Json;

namespace Keycloak.Net.User.Apis.Features.User.RegisterUser;

internal record  RegisterUserRequest()
{
    [JsonPropertyName("username")]
    public string? UserName { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; } = true;
    [JsonPropertyName("credentials")]
    public Credentials[]? Credentials { get; set; }
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
