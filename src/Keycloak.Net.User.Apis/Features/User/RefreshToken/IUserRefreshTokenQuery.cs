using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Token;

namespace Keycloak.Net.User.Apis.Features.User.RefreshToken
{
    internal interface IUserRefreshTokenQuery
    {
        Task<Result<TokenRepresentation?>> RefreshTokenAsync(string clientId, string clientSecret, string refreshToken, CancellationToken cancellationToken = default);
        Task<Result<TokenRepresentation?>> RefreshTokenAsync(string clientId, string refreshToken, CancellationToken cancellationToken = default);
    }
}