using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User.Register;

public interface IRegisterUserWithRealmRoleCommand
{
    Task<Result> RegisterUserWithRealmRole(string username, string password, string email, string firstName, string lastName, string[] rolesName, CancellationToken cancellationToken);
}