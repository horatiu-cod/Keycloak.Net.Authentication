namespace Keycloak.Net.User.Api.Configuration
{
    public record AdminClient
    {
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
    }
}
