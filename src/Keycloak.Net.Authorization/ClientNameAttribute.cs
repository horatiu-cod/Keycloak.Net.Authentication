namespace Keycloak.Net.Authorization;

public class ClientNameAttribute : Attribute
{
    public ClientNameAttribute(string clientName)
    {
        ClientName = clientName;
    }
    public string ClientName { get; set; } = string.Empty;
}
