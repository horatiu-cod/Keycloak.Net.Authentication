using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Token;

namespace Keycloak.Net.User.Apis.Features.User.LoginUser
{
    internal interface IGetUserTokenQuery
    {
        Task<Result<TokenRepresentation?>> LoginUserAsync(string baseAddress, string realmName, string clientId, string clientSecret, string userName, string password, CancellationToken cancellationToken);
    }
}