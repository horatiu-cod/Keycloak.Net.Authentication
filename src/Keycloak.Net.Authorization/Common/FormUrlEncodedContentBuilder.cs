namespace Keycloak.Net.Authorization.Common;

internal class FormUrlEncodedContentBuilder
{
    internal static FormUrlEncodedContent AudienceAccessRequestBody(string clientId)
    {
        var data = new Dictionary<string, string>
        {
            { Constants.GrantType, Constants.GrantTypeValue },
            { Constants.Audience, clientId }
        };

        return new FormUrlEncodedContent(data);
    }
    internal static FormUrlEncodedContent PermissionRequestBody(string clientId, string resource, string scope)
    {
        string? permission;
        if (scope == null)
        {
            permission = $"{resource}";
        }
        else
        {
            permission = $"{resource}#{scope}";
        }
        var data = new Dictionary<string, string>
        {
            { Constants.GrantType, Constants.GrantTypeValue },
            { Constants.Audience, clientId },
            { Constants.Permission, permission }
        };

        return new FormUrlEncodedContent(data);
    }
}
