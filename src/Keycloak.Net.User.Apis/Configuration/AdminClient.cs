namespace Keycloak.Net.User.Apis.Configuration
{
    public record AdminClient
    {
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
    }
}
