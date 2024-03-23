using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User.RegisterUser;

internal interface IRegisterUserRequest
{
    Task RegisterUserWithClientRole(string clientId, string clientSecret, string username, string password, string[] roles);
    Task<Result> RegisterUserWithRealmRole(string url, string clientId, string clientSecret, string username, string password, string[] roles);
}