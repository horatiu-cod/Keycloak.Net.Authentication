namespace Keycloak.Net.User.Apis.Features.Client;

internal record struct ClientDTO
{
    public string ClientId { get; set; }
    public string ClientUuId { get; set; }
}
