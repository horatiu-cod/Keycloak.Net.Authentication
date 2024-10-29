using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.User.AssignRole;

public interface IAssignClientRoleCommand
{
    Task<Result> Handler(string username, string clientId, string[] roleNames, CancellationToken cancellationToken = default);
}
