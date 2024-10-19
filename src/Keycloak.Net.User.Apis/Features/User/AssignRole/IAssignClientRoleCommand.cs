using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User.AssignRole;

public interface IAssignClientRoleCommand
{
    Task<Result> Handler(string username, string clientId, string[] roleNames, CancellationToken cancellationToken = default);
}
