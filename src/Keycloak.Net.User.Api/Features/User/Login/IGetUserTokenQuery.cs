using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Features.Token;

namespace Keycloak.Net.User.Api.Features.User.Login;

public interface IGetUserTokenQuery
{
    Task<Result<TokenRepresentation?>> LoginUserAsync(string clientId, string clientSecret, string userName, string password, CancellationToken cancellationToken = default);
    Task<Result<TokenRepresentation?>> LoginUserAsync(string clientId, string userName, string password, CancellationToken cancellationToken = default);

}