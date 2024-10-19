using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User.Delete;

public interface IDeleteUserCommand
{
    Task<Result> Handler(string username, string realmAdminClientId, string realmAdminClientSecret, CancellationToken cancellationToken = default);
}