namespace Keycloak.Net.Authorization;

[AttributeUsage(AttributeTargets.All)]
public class ClientNameAttribute : Attribute
{
    public ClientNameAttribute(string clientName)
    {
        ClientName = clientName;
    }
    public string ClientName { get; set; } 
}
