namespace Keycloak.Net.Authorization.Representation;

internal record struct UmaRequestRepresentation()
{
    public string ClientId { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public ResponseMode ResponseMode { get; set; }
    public string Ticket { get; set; } = string.Empty;
    public bool SubmitTicket { get; set; } = false;
}
enum ResponseMode
{
    Decision,
    Permission
}