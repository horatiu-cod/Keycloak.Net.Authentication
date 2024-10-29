using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.User.Register;

public interface IRegisterUserWithClientRoleCommand
{
    Task<Result> Handler(string clientId, string username, string password, string email, string firstName, string lastName, string[] rolesName, CancellationToken cancellationToken = default);
}