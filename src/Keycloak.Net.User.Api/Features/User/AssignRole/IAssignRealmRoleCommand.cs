using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.User.AssignRole;

public interface IAssignRealmRoleCommand
{
    Task<Result> Handler(string username, string[] roleNames, CancellationToken cancellationToken = default);
}