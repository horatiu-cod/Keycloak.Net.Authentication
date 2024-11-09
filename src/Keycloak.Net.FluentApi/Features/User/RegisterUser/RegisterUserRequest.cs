using System.Text.Json.Serialization;
using System.Text.Json;

namespace Keycloak.Net.FluentApi.Features.User.RegisterUser;

internal class RegisterUserRequest
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

    public string ToJson() => JsonSerializer.Serialize(this);
    public RegisterUserRequest? FromJson(string keycloakRegisterUserDto) =>
        JsonSerializer.Deserialize<RegisterUserRequest>(keycloakRegisterUserDto, JsonSerializerOptions);
    readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

}
public record Credentials(string Value)
{
    [JsonPropertyName("type")]
    public string? Type { get; set; } = "password";
    [JsonPropertyName("value")]
    public string? Value { get; set; } = Value;
    [JsonPropertyName("temporary")]
    public bool Temporary { get; set; } = false;
}
