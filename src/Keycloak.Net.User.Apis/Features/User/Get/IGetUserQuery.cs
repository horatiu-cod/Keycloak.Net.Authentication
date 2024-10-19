using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User.Get;

public interface IGetUserQuery
{
    Task<Result<GetUserResponse?>> Handler(string username, CancellationToken cancellationToken = default);
}