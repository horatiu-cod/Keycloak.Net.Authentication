using System.Text.Json.Serialization;
using System.Text.Json;

namespace Keycloak.Net.User.Apis.Features.User.RegisterUser;

internal record struct RegisterUserDto()
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; } = true;
    [JsonPropertyName("credentials")]
    public Credentials[]? Credentials { get; set; }
    [JsonPropertyName("username")]
    public string? UserName { get; set; }

    public string ToJson() => JsonSerializer.Serialize(this);
    public RegisterUserDto? FromJson(string keycloakRegisterUserDto) =>
        JsonSerializer.Deserialize<RegisterUserDto>(keycloakRegisterUserDto, JsonSerializerOptions);
    readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

}

public record struct Credentials(string Value)
{
    [JsonPropertyName("type")]
    public string? Type { get; set; } = "password";
    [JsonPropertyName("value")]
    public string? Value { get; set; } = Value;
    [JsonPropertyName("temporary")]
    public bool Temporary { get; set; } = false;
}
