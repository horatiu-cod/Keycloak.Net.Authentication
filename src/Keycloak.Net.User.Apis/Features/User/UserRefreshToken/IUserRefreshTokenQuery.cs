using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Token;

namespace Keycloak.Net.User.Apis.Features.User.UserRefreshToken
{
    internal interface IUserRefreshTokenQuery
    {
        Task<Result<TokenRepresentation?>> RefreshTokenAsync(string baseAddress, string realmName, string clientId, string clientSecret, string refreshtoken, CancellationToken cancellationToken = default);
    }
}