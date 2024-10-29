using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Features.Token;

namespace Keycloak.Net.User.Api.Features.User.RefreshToken
{
    internal interface IUserRefreshTokenQuery
    {
        Task<Result<TokenRepresentation?>> RefreshTokenAsync(string clientId, string clientSecret, string refreshToken, CancellationToken cancellationToken = default);
        Task<Result<TokenRepresentation?>> RefreshTokenAsync(string clientId, string refreshToken, CancellationToken cancellationToken = default);
    }
}