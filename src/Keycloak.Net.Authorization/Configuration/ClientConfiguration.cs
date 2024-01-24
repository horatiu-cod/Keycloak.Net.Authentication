namespace Keycloak.Net.Authorization.Configuration;

public sealed record ClientConfiguration
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}
