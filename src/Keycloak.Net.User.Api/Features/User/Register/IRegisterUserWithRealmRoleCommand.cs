using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.User.Register;

public interface IRegisterUserWithRealmRoleCommand
{
    Task<Result> Handler(string username, string password, string email, string firstName, string lastName, string[] rolesName, CancellationToken cancellationToken = default);
}