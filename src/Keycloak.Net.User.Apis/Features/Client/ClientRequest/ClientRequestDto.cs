namespace Keycloak.Net.User.Apis.Features.Client.ClientRequest;

internal record struct ClientRequestDto
{
    public string ClientId { get; set; }
    public string ClientUuId { get; set; }
}
