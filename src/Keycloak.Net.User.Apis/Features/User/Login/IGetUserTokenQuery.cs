using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Token;

namespace Keycloak.Net.User.Apis.Features.User.Login;

public interface IGetUserTokenQuery
{
    Task<Result<TokenRepresentation?>> LoginUserAsync(string clientId, string clientSecret, string userName, string password, CancellationToken cancellationToken = default);
    Task<Result<TokenRepresentation?>> LoginUserAsync(string clientId, string userName, string password, CancellationToken cancellationToken = default);

}