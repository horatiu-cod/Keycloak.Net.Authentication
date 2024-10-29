using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.User.Get;

public interface IGetUserQuery
{
    Task<Result<GetUserResponse?>> Handler(string username, CancellationToken cancellationToken = default);
}