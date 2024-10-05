using System.Text.Json.Serialization;

namespace Keycloak.Net.User.Apis.Features.User.ResetPassword;

internal record ResetPasswordRequest(string Password)
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "password";
    [JsonPropertyName("temporary")]
    public bool Temporary { get; set; } = false;
    [JsonPropertyName("value")]
    public string Value {  get; set; } = Password;

}
