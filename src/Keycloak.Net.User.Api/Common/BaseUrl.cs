namespace Keycloak.Net.User.Api.Common;

internal static class BaseUrl
{
    public static string AdminUrl(string BaseAddress, string RealmName) => $"{(BaseAddress.EndsWith('/') ? BaseAddress.TrimEnd('/') : BaseAddress)}/admin/realms/{RealmName}";
    public static string RealmUrl(string BaseAddress, string RealmName) => $"{(BaseAddress.EndsWith('/') ? BaseAddress.TrimEnd('/') : BaseAddress)}/realms/{RealmName}";
    public static string TokenUrl(string BaseAddress, string RealmName) => $"{RealmUrl(BaseAddress, RealmName)}/protocol/openid-connect/token";
    public static string LogoutUrl(string BaseAddress, string RealmName) => $"{RealmUrl(BaseAddress, RealmName)}/protocol/openid-connect/logout";
    public static string ResetPasswordUrl(string BaseAddress, string RealmName) => $"{BaseAddress}/realms/{RealmName}/account/credentials/password";
}
