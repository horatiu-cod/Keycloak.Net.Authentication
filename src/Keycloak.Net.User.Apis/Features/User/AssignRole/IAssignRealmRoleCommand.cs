using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User.AssignRole;

public interface IAssignRealmRoleCommand
{
    Task<Result> Handler(string username, string[] roleNames, CancellationToken cancellationToken = default);
}