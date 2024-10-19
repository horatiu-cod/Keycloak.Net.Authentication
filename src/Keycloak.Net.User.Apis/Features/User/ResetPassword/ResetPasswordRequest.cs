using System.Text.Json.Serialization;

namespace Keycloak.Net.User.Apis.Features.User.ResetPassword;

internal record ResetPasswordRequest()
{
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

