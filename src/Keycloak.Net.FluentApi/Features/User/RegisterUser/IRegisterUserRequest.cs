using Keycloak.Net.FluentApi.Common;

namespace Keycloak.Net.FluentApi.Features.User.RegisterUser
{
    internal interface IRegisterUserRequest
    {
        Task<Result> RegisterUserWithClientRole(string url, string uri, string authClientId, string authClientSecret, string username, string password, string[] roles, string clientId, HttpClient httpClient);
        Task<Result> RegisterUserWithoutRole(string url, string uri, string authClientId, string authClientSecret, string username, string password, HttpClient httpClient);
        Task<Result> RegisterUserWithRealmRole(string url, string uri, string clientId, string clientSecret, string username, string password, string[] roles, HttpClient httpClient);
    }
}