namespace Keycloak.Net.User.Apis.Common
{
    internal interface IRequestUrlBuilder
    {
        string TokenApi { get; }
        string AdminApi { get; }
        string LogoutApi { get; }
    }
}