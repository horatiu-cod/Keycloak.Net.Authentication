using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.User.Delete;

public interface IDeleteUserCommand
{
    Task<Result> Handler(string username, string realmAdminClientId, string realmAdminClientSecret, CancellationToken cancellationToken = default);
}