using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User.RegisterUser;

internal interface IRegisterUserCommand
{
    Task<Result> RegisterUser(string baseAddress, string realmName, string clientId, string clientSecret, string username, string password);
    Task<Result> RegisterUserWithClientRole(string baseAddress, string realmName, string clientId, string clientSecret, string clientUuid, string username, string password, string[] rolesName);
    Task<Result> RegisterUserWithRealmRole(string baseAddress, string realmName, string clientId, string clientSecret, string username, string password, string[] rolesName);
} 