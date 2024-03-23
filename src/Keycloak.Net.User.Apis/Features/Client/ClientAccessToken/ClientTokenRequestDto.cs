namespace Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;

internal record struct ClientTokenRequestDto
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}
